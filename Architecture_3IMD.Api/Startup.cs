using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Architecture_3IMD.Controllers;
using Architecture_3IMD.Data;
using Architecture_3IMD.Repositories;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

using System.Net.Http;
using BasisRegisters.Vlaanderen;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;


namespace Architecture_3IMD
{
    public class Startup
    {
        string Db1;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Db1 = configuration.GetConnectionString("SQL");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Notice you don't add IBasisRegisterService directly. Since you add a service for the HttpClient 
            // it depends on it's automatically added with the correct lifecycle.
            // HttpClients are pretty complex in .NET Core.
            services.AddHttpClient<IBasisRegisterService, BasisRegisterService>(
                // since provider is not used we can discard it (i.e. replace it with an underscore)
                // (provider, client) =>

                (_, client) =>
                {
                    // needless to say, better in config. We pass the api baseuri here.
                    client.BaseAddress = new Uri("https://api.basisregisters.vlaanderen.be");
                })
                .AddPolicyHandler(GetRetryPolicy());
            services.AddControllers();
    
            // this helper method says "whenever you need a database context, create one using the options specified in my builder".
            // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
            services.AddDbContextPool<ApplicationDbContext>(    
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        // Replace with your connection string. Should be in your env but for example purposes this is _good enough_ for now
                        //Configuration.GetConnectionString("GlobalDatabase"),
                        Db1,
                        // Replace with your server version and type.
                        
                        new MySqlServerVersion(new Version(10, 4, 11)),
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend)
            ));
            services.Configure<SalesDbContext>(
                Configuration.GetSection("FlowershopSalesDatabaseSettings"));
            services.AddMemoryCache();
            services.AddSingleton<ISalesDbContext>(sp =>
                sp.GetRequiredService<IOptions<SalesDbContext>>().Value);
            services.AddTransient<IBouquetsRepository, BouquetsRepository>();
            services.AddTransient<IStoresRepository, StoresRepository>();
            services.AddTransient<ISalesRepository, SalesRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Flowershop API",
                    Version = "v1",
                    Description = "A platform to manage flowershops. Created by Jasper Peeters, Medina Dadurgova, Marlena Broniewicz and Ward Vandevoort",
                
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flowershop API V1");
            });

           
        } 

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
            // You can just "read" this part of the code, it does what you think it does.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
    }
}

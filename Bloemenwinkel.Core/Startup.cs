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

namespace Architecture_3IMD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            // this helper method says "whenever you need a database context, create one using the options specified in my builder".
            // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
            services.AddDbContextPool<ApplicationDbContext>(    
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        // Replace with your connection string. Should be in your env but for example purposes this is _good enough_ for now
                        "server=localhost;user=root;password=;database=architecture",
                        // Replace with your server version and type.
                        mySqlOptions => mySqlOptions
                            .ServerVersion(new Version(10, 4, 11), ServerType.MySql)
                            .CharSetBehavior(CharSetBehavior.NeverAppend)
            ));
            services.AddTransient<IBouquetsRepository, BouquetsRepository>();
            services.AddTransient<IStoresRepository, StoresRepository>();
            
                    
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

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "getStores",
                    pattern: "getStores",
                    defaults: new {controller = "Home", action = "getAllStores"});

                endpoints.MapControllerRoute(
                    name: "getStore",
                    pattern: "getStore",
                    defaults: new {controller = "Home", action = "getStore"});

                endpoints.MapControllerRoute(
                    name: "postTest",
                    pattern: "postTest",
                    defaults: new {controller = "Home", action = "postTest"});
                
                endpoints.MapControllerRoute(
                    name: "getBouquets",
                    pattern: "getBouquets",
                    defaults: new {controller = "Home", action = "getAllBouquets"});
            });

        } 
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Architecture_3IMD.Migrations
{
    public partial class stores_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: false),
                    StreetNumber = table.Column<string>(maxLength: 255, nullable: false),
                    Region = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Name", "Address", "StreetNumber", "Region" },
                values: new object[,]
                {
                    {1, "Bloemen Ver Elst", "Montystraat", "100", "Tremelo"},
                    {2, "Fleur", "Pastoriestraat", "162", "Tremelo"},
                    {3, "Passie-Flora", "Liersesteenweg", "10", "Mechelen"},
                    {4, "Potvolbloeme", "Stationstraat", "42", "Mechelen"},
                    {5, "Het Bloemke", "Zandpoortvest", "12", "Mechelen"}
                    
                }); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}

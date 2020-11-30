using Microsoft.EntityFrameworkCore.Migrations;

namespace Architecture_3IMD.Migrations
{
    public partial class bouquets_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bouquets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bouquets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bouquets",
                columns: new[] { "Id", "Name", "Price", "Description" },
                values: new object[,]
                {
                    {1, "Roses", 25, "Red roses"},
                    {2, "Orchids", 20, "White orchids"},
                    {3, "Violets", 15, "Violet violets"},
                    {4, "Tulips", 18, "Dutch tulips"},
                    {5, "Crocusses", 17, "A bouquet of crocusses"}
                    
                });   
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bouquets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabasePostgresSQLEFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedTablePeople : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "people_legal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people_legal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_people_legal_people_Id",
                        column: x => x.Id,
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people_physical",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CPF = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people_physical", x => x.Id);
                    table.ForeignKey(
                        name: "FK_people_physical_people_Id",
                        column: x => x.Id,
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "people_legal");

            migrationBuilder.DropTable(
                name: "people_physical");

            migrationBuilder.DropTable(
                name: "people");
        }
    }
}

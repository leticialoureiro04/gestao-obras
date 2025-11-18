using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoObras.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id_Cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NIF = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Morada = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id_Cliente);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Materiais",
                columns: table => new
                {
                    Id_Material = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Stock_Disponivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiais", x => x.Id_Material);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    Id_Obra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Morada = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Ativa = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Id_Cliente = table.Column<int>(type: "int", nullable: true),
                    ClienteId_Cliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.Id_Obra);
                    table.ForeignKey(
                        name: "FK_Obras_Clientes_ClienteId_Cliente",
                        column: x => x.ClienteId_Cliente,
                        principalTable: "Clientes",
                        principalColumn: "Id_Cliente");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaoDeObra",
                columns: table => new
                {
                    Id_Mao_Obra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Horas = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_Obra = table.Column<int>(type: "int", nullable: true),
                    ObraId_Obra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaoDeObra", x => x.Id_Mao_Obra);
                    table.ForeignKey(
                        name: "FK_MaoDeObra_Obras_ObraId_Obra",
                        column: x => x.ObraId_Obra,
                        principalTable: "Obras",
                        principalColumn: "Id_Obra");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Movimentos",
                columns: table => new
                {
                    Id_Movimento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Operacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Data_Operacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_Obra = table.Column<int>(type: "int", nullable: true),
                    ObraId_Obra = table.Column<int>(type: "int", nullable: true),
                    Id_Material = table.Column<int>(type: "int", nullable: true),
                    MaterialId_Material = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentos", x => x.Id_Movimento);
                    table.ForeignKey(
                        name: "FK_Movimentos_Materiais_MaterialId_Material",
                        column: x => x.MaterialId_Material,
                        principalTable: "Materiais",
                        principalColumn: "Id_Material");
                    table.ForeignKey(
                        name: "FK_Movimentos_Obras_ObraId_Obra",
                        column: x => x.ObraId_Obra,
                        principalTable: "Obras",
                        principalColumn: "Id_Obra");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id_Pagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_Obra = table.Column<int>(type: "int", nullable: true),
                    ObraId_Obra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id_Pagamento);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Obras_ObraId_Obra",
                        column: x => x.ObraId_Obra,
                        principalTable: "Obras",
                        principalColumn: "Id_Obra");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MaoDeObra_ObraId_Obra",
                table: "MaoDeObra",
                column: "ObraId_Obra");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_MaterialId_Material",
                table: "Movimentos",
                column: "MaterialId_Material");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentos_ObraId_Obra",
                table: "Movimentos",
                column: "ObraId_Obra");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_ClienteId_Cliente",
                table: "Obras",
                column: "ClienteId_Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ObraId_Obra",
                table: "Pagamentos",
                column: "ObraId_Obra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaoDeObra");

            migrationBuilder.DropTable(
                name: "Movimentos");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Materiais");

            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace umfgcloud.programcaoiii.vendas.api.Migrations
{
    /// <inheritdoc />
    public partial class Migration_1_0_0_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NM_CLIENTE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NR_CPF = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    DS_ENDERECO = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    NR_TELEFONE = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    IN_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DT_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    CD_EAN = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: false),
                    DS_PRODUTO = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    VL_PRECO_COMPRA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VL_PRECO_VENDA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QT_ESTOQUE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IN_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DT_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VENDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    ID_CLIENTE = table.Column<Guid>(type: "char(36)", nullable: false),
                    IN_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DT_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VENDA_CLIENTE_ID_CLIENTE",
                        column: x => x.ID_CLIENTE,
                        principalTable: "CLIENTE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ITEM_VENDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    ID_PRODUTO = table.Column<Guid>(type: "char(36)", nullable: false),
                    VL_ITEM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QT_ITEM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VL_TOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_VENDA = table.Column<Guid>(type: "char(36)", nullable: true),
                    IN_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DT_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_VENDA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ITEM_VENDA_PRODUTO_ID_PRODUTO",
                        column: x => x.ID_PRODUTO,
                        principalTable: "PRODUTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ITEM_VENDA_VENDA_ID_VENDA",
                        column: x => x.ID_VENDA,
                        principalTable: "VENDA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_VENDA_ID_PRODUTO",
                table: "ITEM_VENDA",
                column: "ID_PRODUTO");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_VENDA_ID_VENDA",
                table: "ITEM_VENDA",
                column: "ID_VENDA");

            migrationBuilder.CreateIndex(
                name: "IX_VENDA_ID_CLIENTE",
                table: "VENDA",
                column: "ID_CLIENTE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITEM_VENDA");

            migrationBuilder.DropTable(
                name: "PRODUTO");

            migrationBuilder.DropTable(
                name: "VENDA");

            migrationBuilder.DropTable(
                name: "CLIENTE");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ControleEstoque.API.Migrations
{
    /// <inheritdoc />
    public partial class PopularFormasPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FormasPagamento",
                columns: new[] { "Id", "Nome", "Status" },
                values: new object[,]
                {
                    { 1, "Pix", 0 },
                    { 2, "Cartão de Crédito", 0 },
                    { 3, "Cartão de Débito", 0 },
                    { 4, "Dinheiro", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FormasPagamento",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FormasPagamento",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FormasPagamento",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FormasPagamento",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}

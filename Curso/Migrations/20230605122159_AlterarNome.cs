using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoEFCore.Migrations
{
    /// <inheritdoc />
    public partial class AlterarNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Produtos_ProdutoID",
                table: "PedidoItems");

            migrationBuilder.RenameColumn(
                name: "ProdutoID",
                table: "PedidoItems",
                newName: "ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoItems_ProdutoID",
                table: "PedidoItems",
                newName: "IX_PedidoItems_ProdutoId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clientes",
                newName: "Nome");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Produtos_ProdutoId",
                table: "PedidoItems",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Produtos_ProdutoId",
                table: "PedidoItems");

            migrationBuilder.RenameColumn(
                name: "ProdutoId",
                table: "PedidoItems",
                newName: "ProdutoID");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoItems_ProdutoId",
                table: "PedidoItems",
                newName: "IX_PedidoItems_ProdutoID");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Clientes",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Produtos_ProdutoID",
                table: "PedidoItems",
                column: "ProdutoID",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

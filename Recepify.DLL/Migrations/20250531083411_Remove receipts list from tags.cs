using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recepify.DLL.Migrations
{
    /// <inheritdoc />
    public partial class Removereceiptslistfromtags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeTag");

            migrationBuilder.AddColumn<Guid>(
                name: "RecipeId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_RecipeId",
                table: "Tags",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Recipes_RecipeId",
                table: "Tags",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Recipes_RecipeId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_RecipeId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "RecipeTag",
                columns: table => new
                {
                    ReceiptsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTag", x => new { x.ReceiptsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_RecipeTag_Recipes_ReceiptsId",
                        column: x => x.ReceiptsId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTag_TagsId",
                table: "RecipeTag",
                column: "TagsId");
        }
    }
}

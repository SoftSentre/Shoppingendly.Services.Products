using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "products",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ParentCategoryId = table.Column<Guid>(nullable: true),
                    CategoryName = table.Column<string>(maxLength: 30, nullable: false),
                    CategoryDescription = table.Column<string>(maxLength: 4000, nullable: true),
                    CategoryIconName = table.Column<string>(maxLength: 200, nullable: true),
                    CategoryIconUrl = table.Column<string>(maxLength: 500, nullable: true),
                    IsCategoryIconEmpty = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CreatorRoles",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Creators",
                schema: "products",
                columns: table => new
                {
                    CreatorId = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatorRoleId = table.Column<int>(nullable: false),
                    CreatorName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => x.CreatorId);
                    table.ForeignKey(
                        name: "FK_Creators_CreatorRoles_CreatorRoleId",
                        column: x => x.CreatorRoleId,
                        principalSchema: "products",
                        principalTable: "CreatorRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductCreatorId = table.Column<Guid>(nullable: false),
                    ProductPictureName = table.Column<string>(maxLength: 200, nullable: true),
                    ProductPictureUrl = table.Column<string>(maxLength: 500, nullable: true),
                    IsProductPictureEmpty = table.Column<bool>(nullable: true),
                    ProductName = table.Column<string>(maxLength: 30, nullable: false),
                    ProductProducer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Creators_ProductCreatorId",
                        column: x => x.ProductCreatorId,
                        principalSchema: "products",
                        principalTable: "Creators",
                        principalColumn: "CreatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                schema: "products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "products",
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creators_CreatorRoleId",
                schema: "products",
                table: "Creators",
                column: "CreatorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                schema: "products",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCreatorId",
                schema: "products",
                table: "Products",
                column: "ProductCreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Creators",
                schema: "products");

            migrationBuilder.DropTable(
                name: "CreatorRoles",
                schema: "products");
        }
    }
}

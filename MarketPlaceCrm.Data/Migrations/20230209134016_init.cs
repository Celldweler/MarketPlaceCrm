using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlaceCrm.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ProfileImageUrl = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ShipperName = table.Column<string>(nullable: true),
                    RequiredDate = table.Column<DateTime>(nullable: false),
                    ShippedDate = table.Column<DateTime>(nullable: false),
                    Freight = table.Column<decimal>(nullable: false),
                    ShipAddress = table.Column<string>(nullable: true),
                    ShipCountry = table.Column<string>(nullable: true),
                    ShipCity = table.Column<string>(nullable: true),
                    ShipRegion = table.Column<string>(nullable: true),
                    ShipZipCode = table.Column<string>(nullable: true),
                    ShipperID = table.Column<int>(nullable: true),
                    CustomerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Shipper_ShipperID",
                        column: x => x.ShipperID,
                        principalTable: "Shipper",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    ProductID = table.Column<int>(nullable: true),
                    OrderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "City", "Country", "Created", "Deleted", "Email", "LastName", "Name", "PhoneNumber", "ProfileImageUrl", "Region", "Role", "ZipCode" },
                values: new object[,]
                {
                    { 1, "80198 Metz Wall", "Lehnerburgh", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Nora84@yahoo.com", "Koelpin", "Nora", "499.855.4169", null, null, 0, "61592" },
                    { 2, "558 Izabella Spurs", "Ryanside", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Brandon66@gmail.com", "Walter", "Brandon", "(323) 900-7132 x1769", null, null, 0, "04604-0320" },
                    { 3, "928 Jacobi Falls", "Connellyland", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cora.Friesen79@yahoo.com", "Friesen", "Cora", "1-208-316-7176 x925", null, null, 0, "93737" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Cost", "Created", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 15.21m, new DateTime(2022, 2, 3, 10, 12, 50, 774, DateTimeKind.Unspecified).AddTicks(9821), false, "Assumenda est ut impedit voluptatem et eveniet amet est magnam dolor cupiditate vel reiciendis eos delectus excepturi et.", "Non earum aut officia illo." },
                    { 2, 48.63m, new DateTime(2022, 2, 1, 15, 53, 58, 783, DateTimeKind.Unspecified).AddTicks(7870), false, "Quis magnam omnis sunt excepturi suscipit et ut laborum quae quis facilis qui natus et tempora vel voluptatem nisi sint a doloribus omnis natus.", "Recusandae soluta cumque ut eum." },
                    { 3, 54.10m, new DateTime(2022, 3, 22, 9, 45, 15, 771, DateTimeKind.Unspecified).AddTicks(835), false, "Quia vel a facilis molestiae sint id dignissimos explicabo voluptas consequatur illum cum accusamus eius dolores veniam impedit ullam cum alias accusamus molestiae exercitationem.", "Non ut sunt sunt doloribus." },
                    { 4, 71.89m, new DateTime(2022, 12, 14, 4, 44, 9, 474, DateTimeKind.Unspecified).AddTicks(163), false, "Inventore quis voluptatem quia animi sequi et eos veniam ut quam sed fuga rerum rerum natus eos doloremque.", "Distinctio et consequatur praesentium delectus." },
                    { 5, 70.33m, new DateTime(2022, 8, 7, 11, 46, 29, 63, DateTimeKind.Unspecified).AddTicks(6820), false, "Sequi tempora at nemo quidem quidem officiis nulla possimus nemo unde perferendis eveniet magni voluptates iusto qui aut qui consequatur.", "Temporibus enim repudiandae qui qui." },
                    { 6, 80.73m, new DateTime(2022, 4, 8, 12, 49, 36, 993, DateTimeKind.Unspecified).AddTicks(4497), false, "Minus placeat blanditiis et facere itaque alias sunt consequatur reiciendis voluptatem.", "Ipsam aut sunt omnis assumenda." },
                    { 7, 87.19m, new DateTime(2022, 12, 14, 8, 51, 12, 171, DateTimeKind.Unspecified).AddTicks(1739), false, "Natus tenetur aliquid repudiandae consequatur alias facilis asperiores laborum voluptates facere minus.", "Dolor consectetur in perferendis nulla." },
                    { 8, 29.07m, new DateTime(2022, 3, 1, 14, 15, 9, 103, DateTimeKind.Unspecified).AddTicks(524), false, "A consequuntur suscipit et quam quo nam error necessitatibus ex id dicta qui facilis.", "Est aut aut pariatur soluta." },
                    { 9, 29.30m, new DateTime(2022, 9, 20, 4, 52, 59, 476, DateTimeKind.Unspecified).AddTicks(4928), false, "Reprehenderit distinctio aliquam dolore voluptate et rem libero dolor qui suscipit qui dolor et et et dolores ut laboriosam beatae aut aliquam quia libero alias.", "Vel fugiat facilis et aut." },
                    { 10, 23.04m, new DateTime(2022, 7, 6, 23, 20, 44, 90, DateTimeKind.Unspecified).AddTicks(7095), false, "Aut id cum dolor ipsum et quod explicabo eius quia aut placeat.", "Fuga qui dolor sed illum." }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Created", "CustomerID", "Deleted", "Freight", "RequiredDate", "ShipAddress", "ShipCity", "ShipCountry", "ShipRegion", "ShipZipCode", "ShippedDate", "ShipperID", "ShipperName", "Status" },
                values: new object[,]
                {
                    { 10248, new DateTime(2022, 1, 17, 0, 37, 15, 338, DateTimeKind.Unspecified).AddTicks(3492), 1, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10249, new DateTime(2022, 5, 1, 7, 34, 39, 961, DateTimeKind.Unspecified).AddTicks(3990), 1, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10250, new DateTime(2022, 7, 20, 0, 12, 40, 466, DateTimeKind.Unspecified).AddTicks(425), 1, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10251, new DateTime(2022, 7, 26, 10, 5, 44, 741, DateTimeKind.Unspecified).AddTicks(6058), 2, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10252, new DateTime(2022, 3, 12, 21, 6, 22, 253, DateTimeKind.Unspecified).AddTicks(5243), 2, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10253, new DateTime(2022, 4, 1, 11, 7, 10, 120, DateTimeKind.Unspecified).AddTicks(1292), 3, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 },
                    { 10254, new DateTime(2022, 11, 22, 22, 58, 29, 177, DateTimeKind.Unspecified).AddTicks(4506), 3, false, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Id", "Created", "Deleted", "Description", "ProductId", "Qty" },
                values: new object[,]
                {
                    { 22, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9563), false, "M", 6, 100 },
                    { 23, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9565), false, "L", 6, 100 },
                    { 24, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9568), false, "XL", 6, 100 },
                    { 25, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9573), false, "S", 7, 100 },
                    { 26, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9576), false, "M", 7, 100 },
                    { 27, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9578), false, "L", 7, 100 },
                    { 28, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9580), false, "XL", 7, 100 },
                    { 29, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9585), false, "S", 8, 100 },
                    { 33, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9598), false, "S", 9, 100 },
                    { 31, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9591), false, "L", 8, 100 },
                    { 32, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9593), false, "XL", 8, 100 },
                    { 21, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9560), false, "S", 6, 100 },
                    { 34, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9601), false, "M", 9, 100 },
                    { 35, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9603), false, "L", 9, 100 },
                    { 36, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9606), false, "XL", 9, 100 },
                    { 37, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9611), false, "S", 10, 100 },
                    { 38, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9614), false, "M", 10, 100 },
                    { 30, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9588), false, "M", 8, 100 },
                    { 20, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9555), false, "XL", 5, 100 },
                    { 17, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9546), false, "S", 5, 100 },
                    { 18, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9549), false, "M", 5, 100 },
                    { 1, new DateTime(2023, 2, 9, 15, 40, 16, 370, DateTimeKind.Local).AddTicks(8311), false, "S", 1, 100 },
                    { 2, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9057), false, "M", 1, 100 },
                    { 3, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9103), false, "L", 1, 100 },
                    { 4, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9108), false, "XL", 1, 100 },
                    { 5, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9499), false, "S", 2, 100 },
                    { 6, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9506), false, "M", 2, 100 },
                    { 7, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9509), false, "L", 2, 100 },
                    { 8, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9511), false, "XL", 2, 100 },
                    { 9, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9519), false, "S", 3, 100 },
                    { 10, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9522), false, "M", 3, 100 },
                    { 11, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9525), false, "L", 3, 100 },
                    { 12, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9527), false, "XL", 3, 100 },
                    { 13, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9534), false, "S", 4, 100 },
                    { 14, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9536), false, "M", 4, 100 },
                    { 15, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9539), false, "L", 4, 100 },
                    { 16, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9541), false, "XL", 4, 100 },
                    { 39, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9617), false, "L", 10, 100 },
                    { 19, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9552), false, "L", 5, 100 },
                    { 40, new DateTime(2023, 2, 9, 15, 40, 16, 372, DateTimeKind.Local).AddTicks(9619), false, "XL", 10, 100 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "Discount", "OrderID", "ProductID", "Qty", "Total", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 0m, 10248, 2, 5, 0m, 0m },
                    { 2, 0m, 10248, 3, 4, 0m, 0m },
                    { 5, 0m, 10248, 6, 2, 0m, 0m },
                    { 6, 0m, 10248, 7, 3, 0m, 0m },
                    { 9, 0m, 10248, 6, 4, 0m, 0m },
                    { 10, 0m, 10248, 8, 1, 0m, 0m },
                    { 3, 0m, 10249, 7, 3, 0m, 0m },
                    { 7, 0m, 10249, 8, 2, 0m, 0m },
                    { 8, 0m, 10249, 5, 4, 0m, 0m },
                    { 11, 0m, 10249, 9, 4, 0m, 0m },
                    { 4, 0m, 10250, 10, 3, 0m, 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipperID",
                table: "Orders",
                column: "ShipperID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Shipper");
        }
    }
}

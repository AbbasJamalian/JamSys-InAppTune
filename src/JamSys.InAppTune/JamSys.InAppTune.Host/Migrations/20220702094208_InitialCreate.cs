using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JamSys.InAppTune.Host.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    c_id = table.Column<int>(type: "int", nullable: false),
                    c_w_id = table.Column<int>(type: "int", nullable: false),
                    c_d_id = table.Column<short>(type: "smallint", nullable: false),
                    c_since = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    c_payment_cnt = table.Column<short>(type: "smallint", nullable: false),
                    c_delivery_cnt = table.Column<short>(type: "smallint", nullable: false),
                    c_first = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_middle = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_last = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_street_1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_street_2 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_city = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_state = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_zip = table.Column<string>(type: "char(9)", fixedLength: true, maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_phone = table.Column<string>(type: "char(16)", fixedLength: true, maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_credit = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    c_credit_lim = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    c_discount = table.Column<decimal>(type: "decimal(4,4)", precision: 4, scale: 4, nullable: false),
                    c_balance = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    c_ytd_payment = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    c_data = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("customer_i1", x => new { x.c_w_id, x.c_d_id, x.c_id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "district",
                columns: table => new
                {
                    d_w_id = table.Column<int>(type: "int", nullable: false),
                    d_id = table.Column<short>(type: "smallint", nullable: false),
                    d_next_o_id = table.Column<int>(type: "int", nullable: false),
                    d_ytd = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    d_tax = table.Column<decimal>(type: "decimal(4,4)", precision: 4, scale: 4, nullable: false),
                    d_name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    d_street_1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    d_street_2 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    d_city = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    d_state = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    d_zip = table.Column<string>(type: "char(9)", fixedLength: true, maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("district_i1", x => new { x.d_w_id, x.d_id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "history",
                columns: table => new
                {
                    h_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    h_c_id = table.Column<int>(type: "int", nullable: true),
                    h_c_w_id = table.Column<int>(type: "int", nullable: false),
                    h_w_id = table.Column<int>(type: "int", nullable: false),
                    h_c_d_id = table.Column<short>(type: "smallint", nullable: false),
                    h_d_id = table.Column<short>(type: "smallint", nullable: false),
                    h_amount = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    h_data = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    i_id = table.Column<int>(type: "int", nullable: false),
                    i_im_id = table.Column<int>(type: "int", nullable: false),
                    i_name = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    i_price = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    i_data = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("item_i1", x => x.i_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "new_order",
                columns: table => new
                {
                    no_w_id = table.Column<int>(type: "int", nullable: false),
                    no_o_id = table.Column<int>(type: "int", nullable: false),
                    no_d_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("new_order_i1", x => new { x.no_w_id, x.no_d_id, x.no_o_id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order_line",
                columns: table => new
                {
                    ol_o_id = table.Column<int>(type: "int", nullable: false),
                    ol_w_id = table.Column<int>(type: "int", nullable: false),
                    ol_d_id = table.Column<short>(type: "smallint", nullable: false),
                    ol_number = table.Column<short>(type: "smallint", nullable: false),
                    ol_delivery_d = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ol_i_id = table.Column<int>(type: "int", nullable: false),
                    ol_supply_w_id = table.Column<int>(type: "int", nullable: false),
                    ol_quantity = table.Column<short>(type: "smallint", nullable: false),
                    ol_amount = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    ol_dist_info = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_line_i1", x => new { x.ol_w_id, x.ol_d_id, x.ol_o_id, x.ol_number });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    o_id = table.Column<int>(type: "int", nullable: false),
                    o_w_id = table.Column<int>(type: "int", nullable: false),
                    o_d_id = table.Column<short>(type: "smallint", nullable: false),
                    o_entry_d = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    o_c_id = table.Column<int>(type: "int", nullable: false),
                    o_carrier_id = table.Column<short>(type: "smallint", nullable: true),
                    o_ol_cnt = table.Column<short>(type: "smallint", nullable: false),
                    o_all_local = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_i1", x => new { x.o_w_id, x.o_d_id, x.o_id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stock",
                columns: table => new
                {
                    s_i_id = table.Column<int>(type: "int", nullable: false),
                    s_w_id = table.Column<int>(type: "int", nullable: false),
                    s_ytd = table.Column<int>(type: "int", nullable: false),
                    s_quantity = table.Column<short>(type: "smallint", nullable: false),
                    s_order_cnt = table.Column<short>(type: "smallint", nullable: false),
                    s_remote_cnt = table.Column<short>(type: "smallint", nullable: false),
                    s_dist_01 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_02 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_03 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_04 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_05 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_06 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_07 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_08 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_09 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_dist_10 = table.Column<string>(type: "char(24)", fixedLength: true, maxLength: 24, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    s_data = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("stock_i1", x => new { x.s_w_id, x.s_i_id });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    w_id = table.Column<int>(type: "int", nullable: false),
                    w_name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_street_1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_street_2 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_city = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_state = table.Column<string>(type: "char(2)", fixedLength: true, maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_zip = table.Column<string>(type: "char(9)", fixedLength: true, maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    w_tax = table.Column<decimal>(type: "decimal(4,4)", precision: 4, scale: 4, nullable: false),
                    w_ytd = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouse_i1", x => x.w_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "customer_i2",
                table: "customer",
                columns: new[] { "c_w_id", "c_d_id", "c_last", "c_first", "c_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "orders_i2",
                table: "orders",
                columns: new[] { "o_w_id", "o_d_id", "o_c_id", "o_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "district");

            migrationBuilder.DropTable(
                name: "history");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "new_order");

            migrationBuilder.DropTable(
                name: "order_line");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "stock");

            migrationBuilder.DropTable(
                name: "warehouse");
        }
    }
}

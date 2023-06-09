﻿// <auto-generated />
using System;
using JamSys.InAppTune.Host.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JamSys.InAppTune.Host.Migrations
{
    [DbContext(typeof(TpccContext))]
    partial class TpccContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.Customer", b =>
                {
                    b.Property<int>("CWId")
                        .HasColumnType("int")
                        .HasColumnName("c_w_id");

                    b.Property<short>("CDId")
                        .HasColumnType("smallint")
                        .HasColumnName("c_d_id");

                    b.Property<int>("CId")
                        .HasColumnType("int")
                        .HasColumnName("c_id");

                    b.Property<decimal>("CBalance")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("c_balance");

                    b.Property<string>("CCity")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("c_city");

                    b.Property<string>("CCredit")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)")
                        .HasColumnName("c_credit")
                        .IsFixedLength();

                    b.Property<decimal>("CCreditLim")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("c_credit_lim");

                    b.Property<string>("CData")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("c_data");

                    b.Property<short>("CDeliveryCnt")
                        .HasColumnType("smallint")
                        .HasColumnName("c_delivery_cnt");

                    b.Property<decimal>("CDiscount")
                        .HasPrecision(4, 4)
                        .HasColumnType("decimal(4,4)")
                        .HasColumnName("c_discount");

                    b.Property<string>("CFirst")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("c_first");

                    b.Property<string>("CLast")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("c_last");

                    b.Property<string>("CMiddle")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)")
                        .HasColumnName("c_middle")
                        .IsFixedLength();

                    b.Property<short>("CPaymentCnt")
                        .HasColumnType("smallint")
                        .HasColumnName("c_payment_cnt");

                    b.Property<string>("CPhone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("char(16)")
                        .HasColumnName("c_phone")
                        .IsFixedLength();

                    b.Property<DateTime>("CSince")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("c_since");

                    b.Property<string>("CState")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)")
                        .HasColumnName("c_state")
                        .IsFixedLength();

                    b.Property<string>("CStreet1")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("c_street_1");

                    b.Property<string>("CStreet2")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("c_street_2");

                    b.Property<decimal>("CYtdPayment")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("c_ytd_payment");

                    b.Property<string>("CZip")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("char(9)")
                        .HasColumnName("c_zip")
                        .IsFixedLength();

                    b.HasKey("CWId", "CDId", "CId")
                        .HasName("customer_i1");

                    b.HasIndex(new[] { "CWId", "CDId", "CLast", "CFirst", "CId" }, "customer_i2")
                        .IsUnique();

                    b.ToTable("customer", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.District", b =>
                {
                    b.Property<int>("DWId")
                        .HasColumnType("int")
                        .HasColumnName("d_w_id");

                    b.Property<short>("DId")
                        .HasColumnType("smallint")
                        .HasColumnName("d_id");

                    b.Property<string>("DCity")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("d_city");

                    b.Property<string>("DName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("d_name");

                    b.Property<int>("DNextOId")
                        .HasColumnType("int")
                        .HasColumnName("d_next_o_id");

                    b.Property<string>("DState")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)")
                        .HasColumnName("d_state")
                        .IsFixedLength();

                    b.Property<string>("DStreet1")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("d_street_1");

                    b.Property<string>("DStreet2")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("d_street_2");

                    b.Property<decimal>("DTax")
                        .HasPrecision(4, 4)
                        .HasColumnType("decimal(4,4)")
                        .HasColumnName("d_tax");

                    b.Property<decimal>("DYtd")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("d_ytd");

                    b.Property<string>("DZip")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("char(9)")
                        .HasColumnName("d_zip")
                        .IsFixedLength();

                    b.HasKey("DWId", "DId")
                        .HasName("district_i1");

                    b.ToTable("district", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.History", b =>
                {
                    b.Property<decimal>("HAmount")
                        .HasPrecision(6, 2)
                        .HasColumnType("decimal(6,2)")
                        .HasColumnName("h_amount");

                    b.Property<short>("HCDId")
                        .HasColumnType("smallint")
                        .HasColumnName("h_c_d_id");

                    b.Property<int?>("HCId")
                        .HasColumnType("int")
                        .HasColumnName("h_c_id");

                    b.Property<int>("HCWId")
                        .HasColumnType("int")
                        .HasColumnName("h_c_w_id");

                    b.Property<short>("HDId")
                        .HasColumnType("smallint")
                        .HasColumnName("h_d_id");

                    b.Property<string>("HData")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)")
                        .HasColumnName("h_data");

                    b.Property<DateTime>("HDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("h_date");

                    b.Property<int>("HWId")
                        .HasColumnType("int")
                        .HasColumnName("h_w_id");

                    b.ToTable("history", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.Item", b =>
                {
                    b.Property<int>("IId")
                        .HasColumnType("int")
                        .HasColumnName("i_id");

                    b.Property<string>("IData")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("i_data");

                    b.Property<int>("IImId")
                        .HasColumnType("int")
                        .HasColumnName("i_im_id");

                    b.Property<string>("IName")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)")
                        .HasColumnName("i_name");

                    b.Property<decimal>("IPrice")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("i_price");

                    b.HasKey("IId")
                        .HasName("item_i1");

                    b.ToTable("item", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.NewOrder", b =>
                {
                    b.Property<int>("NoWId")
                        .HasColumnType("int")
                        .HasColumnName("no_w_id");

                    b.Property<short>("NoDId")
                        .HasColumnType("smallint")
                        .HasColumnName("no_d_id");

                    b.Property<int>("NoOId")
                        .HasColumnType("int")
                        .HasColumnName("no_o_id");

                    b.HasKey("NoWId", "NoDId", "NoOId")
                        .HasName("new_order_i1");

                    b.ToTable("new_order", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.Order", b =>
                {
                    b.Property<int>("OWId")
                        .HasColumnType("int")
                        .HasColumnName("o_w_id");

                    b.Property<short>("ODId")
                        .HasColumnType("smallint")
                        .HasColumnName("o_d_id");

                    b.Property<int>("OId")
                        .HasColumnType("int")
                        .HasColumnName("o_id");

                    b.Property<short>("OAllLocal")
                        .HasColumnType("smallint")
                        .HasColumnName("o_all_local");

                    b.Property<int>("OCId")
                        .HasColumnType("int")
                        .HasColumnName("o_c_id");

                    b.Property<short?>("OCarrierId")
                        .HasColumnType("smallint")
                        .HasColumnName("o_carrier_id");

                    b.Property<DateTime>("OEntryD")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("o_entry_d");

                    b.Property<short>("OOlCnt")
                        .HasColumnType("smallint")
                        .HasColumnName("o_ol_cnt");

                    b.HasKey("OWId", "ODId", "OId")
                        .HasName("orders_i1");

                    b.HasIndex(new[] { "OWId", "ODId", "OCId", "OId" }, "orders_i2")
                        .IsUnique();

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.OrderLine", b =>
                {
                    b.Property<int>("OlWId")
                        .HasColumnType("int")
                        .HasColumnName("ol_w_id");

                    b.Property<short>("OlDId")
                        .HasColumnType("smallint")
                        .HasColumnName("ol_d_id");

                    b.Property<int>("OlOId")
                        .HasColumnType("int")
                        .HasColumnName("ol_o_id");

                    b.Property<short>("OlNumber")
                        .HasColumnType("smallint")
                        .HasColumnName("ol_number");

                    b.Property<decimal?>("OlAmount")
                        .HasPrecision(6, 2)
                        .HasColumnType("decimal(6,2)")
                        .HasColumnName("ol_amount");

                    b.Property<DateTime?>("OlDeliveryD")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ol_delivery_d");

                    b.Property<string>("OlDistInfo")
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("ol_dist_info")
                        .IsFixedLength();

                    b.Property<int>("OlIId")
                        .HasColumnType("int")
                        .HasColumnName("ol_i_id");

                    b.Property<short>("OlQuantity")
                        .HasColumnType("smallint")
                        .HasColumnName("ol_quantity");

                    b.Property<int>("OlSupplyWId")
                        .HasColumnType("int")
                        .HasColumnName("ol_supply_w_id");

                    b.HasKey("OlWId", "OlDId", "OlOId", "OlNumber")
                        .HasName("order_line_i1");

                    b.ToTable("order_line", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.Stock", b =>
                {
                    b.Property<int>("SWId")
                        .HasColumnType("int")
                        .HasColumnName("s_w_id");

                    b.Property<int>("SIId")
                        .HasColumnType("int")
                        .HasColumnName("s_i_id");

                    b.Property<string>("SData")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("s_data");

                    b.Property<string>("SDist01")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_01")
                        .IsFixedLength();

                    b.Property<string>("SDist02")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_02")
                        .IsFixedLength();

                    b.Property<string>("SDist03")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_03")
                        .IsFixedLength();

                    b.Property<string>("SDist04")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_04")
                        .IsFixedLength();

                    b.Property<string>("SDist05")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_05")
                        .IsFixedLength();

                    b.Property<string>("SDist06")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_06")
                        .IsFixedLength();

                    b.Property<string>("SDist07")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_07")
                        .IsFixedLength();

                    b.Property<string>("SDist08")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_08")
                        .IsFixedLength();

                    b.Property<string>("SDist09")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_09")
                        .IsFixedLength();

                    b.Property<string>("SDist10")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("char(24)")
                        .HasColumnName("s_dist_10")
                        .IsFixedLength();

                    b.Property<short>("SOrderCnt")
                        .HasColumnType("smallint")
                        .HasColumnName("s_order_cnt");

                    b.Property<short>("SQuantity")
                        .HasColumnType("smallint")
                        .HasColumnName("s_quantity");

                    b.Property<short>("SRemoteCnt")
                        .HasColumnType("smallint")
                        .HasColumnName("s_remote_cnt");

                    b.Property<int>("SYtd")
                        .HasColumnType("int")
                        .HasColumnName("s_ytd");

                    b.HasKey("SWId", "SIId")
                        .HasName("stock_i1");

                    b.ToTable("stock", (string)null);
                });

            modelBuilder.Entity("JamSys.InAppTune.Host.Data.Warehouse", b =>
                {
                    b.Property<int>("WId")
                        .HasColumnType("int")
                        .HasColumnName("w_id");

                    b.Property<string>("WCity")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("w_city");

                    b.Property<string>("WName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("w_name");

                    b.Property<string>("WState")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)")
                        .HasColumnName("w_state")
                        .IsFixedLength();

                    b.Property<string>("WStreet1")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("w_street_1");

                    b.Property<string>("WStreet2")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("w_street_2");

                    b.Property<decimal>("WTax")
                        .HasPrecision(4, 4)
                        .HasColumnType("decimal(4,4)")
                        .HasColumnName("w_tax");

                    b.Property<decimal>("WYtd")
                        .HasPrecision(12, 2)
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("w_ytd");

                    b.Property<string>("WZip")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("char(9)")
                        .HasColumnName("w_zip")
                        .IsFixedLength();

                    b.HasKey("WId")
                        .HasName("warehouse_i1");

                    b.ToTable("warehouse", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

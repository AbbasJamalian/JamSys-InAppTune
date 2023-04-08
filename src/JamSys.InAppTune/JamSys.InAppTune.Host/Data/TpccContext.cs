using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace JamSys.InAppTune.Host.Data
{
    public class TpccContext : TunerDbContext
    {
        public TpccContext()
        {

        }
        public TpccContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<NewOrder> NewOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => new { e.CWId, e.CDId, e.CId })
                    .HasName("customer_i1");

                entity.ToTable("customer");

                entity.HasIndex(e => new { e.CWId, e.CDId, e.CLast, e.CFirst, e.CId }, "customer_i2")
                    .IsUnique();

                entity.Property(e => e.CWId).HasColumnName("c_w_id");

                entity.Property(e => e.CDId).HasColumnName("c_d_id");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.CBalance)
                    .HasPrecision(12, 2)
                    .HasColumnName("c_balance");

                entity.Property(e => e.CCity)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("c_city");

                entity.Property(e => e.CCredit)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("c_credit")
                    .IsFixedLength();

                entity.Property(e => e.CCreditLim)
                    .HasPrecision(12, 2)
                    .HasColumnName("c_credit_lim");

                entity.Property(e => e.CData)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("c_data");

                entity.Property(e => e.CDeliveryCnt).HasColumnName("c_delivery_cnt");

                entity.Property(e => e.CDiscount)
                    .HasPrecision(4, 4)
                    .HasColumnName("c_discount");

                entity.Property(e => e.CFirst)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("c_first");

                entity.Property(e => e.CLast)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("c_last");

                entity.Property(e => e.CMiddle)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("c_middle")
                    .IsFixedLength();

                entity.Property(e => e.CPaymentCnt).HasColumnName("c_payment_cnt");

                entity.Property(e => e.CPhone)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("c_phone")
                    .IsFixedLength();

                entity.Property(e => e.CSince).HasColumnName("c_since");

                entity.Property(e => e.CState)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("c_state")
                    .IsFixedLength();

                entity.Property(e => e.CStreet1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("c_street_1");

                entity.Property(e => e.CStreet2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("c_street_2");

                entity.Property(e => e.CYtdPayment)
                    .HasPrecision(12, 2)
                    .HasColumnName("c_ytd_payment");

                entity.Property(e => e.CZip)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("c_zip")
                    .IsFixedLength();
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => new { e.DWId, e.DId })
                    .HasName("district_i1");

                entity.ToTable("district");

                entity.Property(e => e.DWId).HasColumnName("d_w_id");

                entity.Property(e => e.DId).HasColumnName("d_id");

                entity.Property(e => e.DCity)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("d_city");

                entity.Property(e => e.DName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("d_name");

                entity.Property(e => e.DNextOId).HasColumnName("d_next_o_id");

                entity.Property(e => e.DState)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("d_state")
                    .IsFixedLength();

                entity.Property(e => e.DStreet1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("d_street_1");

                entity.Property(e => e.DStreet2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("d_street_2");

                entity.Property(e => e.DTax)
                    .HasPrecision(4, 4)
                    .HasColumnName("d_tax");

                entity.Property(e => e.DYtd)
                    .HasPrecision(12, 2)
                    .HasColumnName("d_ytd");

                entity.Property(e => e.DZip)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("d_zip")
                    .IsFixedLength();
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("history");

                entity.Property(e => e.HAmount)
                    .HasPrecision(6, 2)
                    .HasColumnName("h_amount");

                entity.Property(e => e.HCDId).HasColumnName("h_c_d_id");

                entity.Property(e => e.HCId).HasColumnName("h_c_id");

                entity.Property(e => e.HCWId).HasColumnName("h_c_w_id");

                entity.Property(e => e.HDId).HasColumnName("h_d_id");

                entity.Property(e => e.HData)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("h_data");

                entity.Property(e => e.HDate).HasColumnName("h_date");

                entity.Property(e => e.HWId).HasColumnName("h_w_id");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.IId)
                    .HasName("item_i1");

                entity.ToTable("item");

                entity.Property(e => e.IId)
                    .ValueGeneratedNever()
                    .HasColumnName("i_id");

                entity.Property(e => e.IData)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("i_data");

                entity.Property(e => e.IImId).HasColumnName("i_im_id");

                entity.Property(e => e.IName)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("i_name");

                entity.Property(e => e.IPrice)
                    .HasPrecision(5, 2)
                    .HasColumnName("i_price");

                entity.HasIndex(i => i.IName);
            });

            modelBuilder.Entity<NewOrder>(entity =>
            {
                entity.HasKey(e => new { e.NoWId, e.NoDId, e.NoOId })
                    .HasName("new_order_i1");

                entity.ToTable("new_order");

                entity.Property(e => e.NoWId).HasColumnName("no_w_id");

                entity.Property(e => e.NoDId).HasColumnName("no_d_id");

                entity.Property(e => e.NoOId).HasColumnName("no_o_id");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.OWId, e.ODId, e.OId })
                    .HasName("orders_i1");

                entity.ToTable("orders");

                entity.HasIndex(e => new { e.OWId, e.ODId, e.OCId, e.OId }, "orders_i2")
                    .IsUnique();

                entity.Property(e => e.OWId).HasColumnName("o_w_id");

                entity.Property(e => e.ODId).HasColumnName("o_d_id");

                entity.Property(e => e.OId).HasColumnName("o_id");

                entity.Property(e => e.OAllLocal).HasColumnName("o_all_local");

                entity.Property(e => e.OCId).HasColumnName("o_c_id");

                entity.Property(e => e.OCarrierId).HasColumnName("o_carrier_id");

                entity.Property(e => e.OEntryD).HasColumnName("o_entry_d");

                entity.Property(e => e.OOlCnt).HasColumnName("o_ol_cnt");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasKey(e => new { e.OlWId, e.OlDId, e.OlOId, e.OlNumber })
                    .HasName("order_line_i1");

                entity.ToTable("order_line");

                entity.Property(e => e.OlWId).HasColumnName("ol_w_id");

                entity.Property(e => e.OlDId).HasColumnName("ol_d_id");

                entity.Property(e => e.OlOId).HasColumnName("ol_o_id");

                entity.Property(e => e.OlNumber).HasColumnName("ol_number");

                entity.Property(e => e.OlAmount)
                    .HasPrecision(6, 2)
                    .HasColumnName("ol_amount");

                entity.Property(e => e.OlDeliveryD).HasColumnName("ol_delivery_d");

                entity.Property(e => e.OlDistInfo)
                    .HasMaxLength(24)
                    .HasColumnName("ol_dist_info")
                    .IsFixedLength();

                entity.Property(e => e.OlIId).HasColumnName("ol_i_id");

                entity.Property(e => e.OlQuantity).HasColumnName("ol_quantity");

                entity.Property(e => e.OlSupplyWId).HasColumnName("ol_supply_w_id");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => new { e.SWId, e.SIId })
                    .HasName("stock_i1");

                entity.ToTable("stock");

                entity.Property(e => e.SWId).HasColumnName("s_w_id");

                entity.Property(e => e.SIId).HasColumnName("s_i_id");

                entity.Property(e => e.SData)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("s_data");

                entity.Property(e => e.SDist01)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_01")
                    .IsFixedLength();

                entity.Property(e => e.SDist02)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_02")
                    .IsFixedLength();

                entity.Property(e => e.SDist03)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_03")
                    .IsFixedLength();

                entity.Property(e => e.SDist04)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_04")
                    .IsFixedLength();

                entity.Property(e => e.SDist05)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_05")
                    .IsFixedLength();

                entity.Property(e => e.SDist06)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_06")
                    .IsFixedLength();

                entity.Property(e => e.SDist07)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_07")
                    .IsFixedLength();

                entity.Property(e => e.SDist08)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_08")
                    .IsFixedLength();

                entity.Property(e => e.SDist09)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_09")
                    .IsFixedLength();

                entity.Property(e => e.SDist10)
                    .IsRequired()
                    .HasMaxLength(24)
                    .HasColumnName("s_dist_10")
                    .IsFixedLength();

                entity.Property(e => e.SOrderCnt).HasColumnName("s_order_cnt");

                entity.Property(e => e.SQuantity).HasColumnName("s_quantity");

                entity.Property(e => e.SRemoteCnt).HasColumnName("s_remote_cnt");

                entity.Property(e => e.SYtd).HasColumnName("s_ytd");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.WId)
                    .HasName("warehouse_i1");

                entity.ToTable("warehouse");

                entity.Property(e => e.WId)
                    .ValueGeneratedNever()
                    .HasColumnName("w_id");

                entity.Property(e => e.WCity)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("w_city");

                entity.Property(e => e.WName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("w_name");

                entity.Property(e => e.WState)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("w_state")
                    .IsFixedLength();

                entity.Property(e => e.WStreet1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("w_street_1");

                entity.Property(e => e.WStreet2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("w_street_2");

                entity.Property(e => e.WTax)
                    .HasPrecision(4, 4)
                    .HasColumnName("w_tax");

                entity.Property(e => e.WYtd)
                    .HasPrecision(12, 2)
                    .HasColumnName("w_ytd");

                entity.Property(e => e.WZip)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("w_zip")
                    .IsFixedLength();
            });

        }
    }
}

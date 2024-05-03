using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Repair3.Models;

namespace Repair3;

public partial class Repair3Context : DbContext
{
    public Repair3Context()
    {
    }

    public Repair3Context(DbContextOptions<Repair3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserOrder> UserOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=Faza2005;database=Repair3", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("PRIMARY");

            entity.ToTable("part");

            entity.HasIndex(e => e.PartId, "Part_Id").IsUnique();

            entity.Property(e => e.PartId).HasColumnName("Part_Id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.QuantityInStock).HasColumnName("Quantity_In_Stock");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasMany(d => d.UserOrders).WithMany(p => p.Parts)
                .UsingEntity<Dictionary<string, object>>(
                    "PartOrder",
                    r => r.HasOne<UserOrder>().WithMany()
                        .HasForeignKey("UserOrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("part_order_ibfk_2"),
                    l => l.HasOne<Part>().WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("part_order_ibfk_1"),
                    j =>
                    {
                        j.HasKey("PartId", "UserOrderId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("part_order");
                        j.HasIndex(new[] { "UserOrderId" }, "User_Order_Id");
                        j.IndexerProperty<int>("PartId").HasColumnName("Part_Id");
                        j.IndexerProperty<int>("UserOrderId").HasColumnName("User_Order_Id");
                    });
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PRIMARY");

            entity.ToTable("report");

            entity.HasIndex(e => e.ReportId, "Report_Id").IsUnique();

            entity.Property(e => e.ReportId).HasColumnName("Report_Id");
            entity.Property(e => e.MainfactionCause)
                .HasColumnType("text")
                .HasColumnName("Mainfaction_Cause");
            entity.Property(e => e.MaterualsSpend)
                .HasColumnType("text")
                .HasColumnName("Materuals_Spend");
            entity.Property(e => e.MoneySpend).HasColumnName("Money_Spend");
            entity.Property(e => e.ProvidedAssistance)
                .HasColumnType("text")
                .HasColumnName("Provided_Assistance");
            entity.Property(e => e.TimeSpend).HasColumnName("Time_Spend");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("request");

            entity.HasIndex(e => e.ExecutorId, "Executor_Id");

            entity.HasIndex(e => e.ReportId, "Report_Id");

            entity.HasIndex(e => e.RequestId, "Request_Id").IsUnique();

            entity.HasIndex(e => e.StatusId, "Status_Id");

            entity.Property(e => e.RequestId).HasColumnName("Request_Id");
            entity.Property(e => e.CreationDate).HasColumnName("Creation_Date");
            entity.Property(e => e.ExecutorComment)
                .HasColumnType("text")
                .HasColumnName("Executor_Comment");
            entity.Property(e => e.ExecutorId).HasColumnName("Executor_Id");
            entity.Property(e => e.FaultType)
                .HasMaxLength(50)
                .HasColumnName("Fault_Type");
            entity.Property(e => e.ReportId).HasColumnName("Report_Id");
            entity.Property(e => e.ServiceType)
                .HasMaxLength(50)
                .HasColumnName("Service_Type");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");

            entity.HasOne(d => d.Executor).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ExecutorId)
                .HasConstraintName("request_ibfk_1");

            entity.HasOne(d => d.Report).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("request_ibfk_3");

            entity.HasOne(d => d.Status).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_ibfk_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleId, "Role_Id").IsUnique();

            entity.HasIndex(e => e.Title, "Title").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("status");

            entity.HasIndex(e => e.StatusId, "Status_Id").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "Role_Id");

            entity.HasIndex(e => e.UserId, "User_Id").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Login).HasMaxLength(80);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(11);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<UserOrder>(entity =>
        {
            entity.HasKey(e => e.UserOrderId).HasName("PRIMARY");

            entity.ToTable("user_order");

            entity.HasIndex(e => e.RequestId, "Request_Id");

            entity.HasIndex(e => e.StatusId, "Status_Id");

            entity.HasIndex(e => e.UserOrderId, "User_Order_Id").IsUnique();

            entity.Property(e => e.UserOrderId).HasColumnName("User_Order_Id");
            entity.Property(e => e.CreationDate).HasColumnName("Creation_Date");
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.RequestId).HasColumnName("Request_Id");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");

            entity.HasOne(d => d.Request).WithMany(p => p.UserOrders)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_order_ibfk_1");

            entity.HasOne(d => d.Status).WithMany(p => p.UserOrders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_order_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(OrderingDbContext))]
    [Migration("20210324174143_AddingResolutionToContext")]
    partial class AddingResolutionToContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ordering")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GigId")
                        .HasColumnType("int");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OrderedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("RequirementId")
                        .HasColumnType("int");

                    b.Property<int>("ResolutionId")
                        .HasColumnType("int");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderNumber")
                        .IsUnique();

                    b.HasIndex("ResolutionId")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Requirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(2500)
                        .HasColumnType("nvarchar(2500)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Requirements");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Resolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Resolutions");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("OrderState")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderStates");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.HasOne("Ordering.Domain.Entities.Resolution", "Resolution")
                        .WithOne("Order")
                        .HasForeignKey("Ordering.Domain.Entities.Order", "ResolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resolution");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Requirement", b =>
                {
                    b.HasOne("Ordering.Domain.Entities.Order", "Order")
                        .WithOne("Requirement")
                        .HasForeignKey("Ordering.Domain.Entities.Requirement", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.State", b =>
                {
                    b.HasOne("Ordering.Domain.Entities.Order", "Order")
                        .WithMany("OrderStates")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderStates");

                    b.Navigation("Requirement");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Resolution", b =>
                {
                    b.Navigation("Order");
                });
#pragma warning restore 612, 618
        }
    }
}
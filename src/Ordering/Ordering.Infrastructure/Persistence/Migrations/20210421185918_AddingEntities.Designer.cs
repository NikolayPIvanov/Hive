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
    [Migration("20210421185918_AddingEntities")]
    partial class AddingEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ordering")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ordering.Domain.Entities.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuyerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<string>("SellerUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("OrderNumber")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Resolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId1")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderId1");

                    b.ToTable("Resolutions");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.HasOne("Ordering.Domain.Entities.Buyer", null)
                        .WithMany("Orders")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Ordering.Domain.ValueObjects.Requirement", "Requirement", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Details")
                                .IsRequired()
                                .HasMaxLength(2500)
                                .HasColumnType("nvarchar(2500)");

                            b1.Property<int>("OrderId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("OrderId")
                                .IsUnique();

                            b1.ToTable("requirements");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsMany("Ordering.Domain.ValueObjects.State", "OrderStates", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("OrderId")
                                .HasColumnType("int");

                            b1.Property<int>("OrderState")
                                .HasColumnType("int");

                            b1.Property<string>("Reason")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id");

                            b1.HasIndex("OrderId");

                            b1.ToTable("State");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("OrderStates");

                    b.Navigation("Requirement");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Resolution", b =>
                {
                    b.HasOne("Ordering.Domain.Entities.Order", null)
                        .WithMany("Resolutions")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ordering.Domain.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId1");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Buyer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.Navigation("Resolutions");
                });
#pragma warning restore 612, 618
        }
    }
}

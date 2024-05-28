﻿// <auto-generated />
using System;
using Data_layer.Context.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_layer.Migrations
{
    [DbContext(typeof(MyDbcontextSofani))]
    [Migration("20240426134938_intile")]
    partial class intile
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data_layer.Context.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quantityProduct")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Data_layer.Context.CustomerEntityModel", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("line1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("recipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Data_layer.Context.Data.CartEnityModel", b =>
                {
                    b.Property<Guid>("cartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Kilo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("productId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("cartId");

                    b.HasIndex("UserId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("Data_layer.Context.Data.UserRegistrationEntityModel", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BTW")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BedrijfsNaam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("KvkNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserRegistration");
                });

            modelBuilder.Entity("Data_layer.Context.Data.WishlistEntityModel", b =>
                {
                    b.Property<Guid>("WishlistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WishlistId");

                    b.HasIndex("UserId");

                    b.ToTable("WishlistEntityModel");
                });

            modelBuilder.Entity("Data_layer.Context.LoginEnitiyModel", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"));

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("useName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginId");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("Data_layer.Context.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("OrderNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("UserRegistrationEntityModelUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("UserRegistrationEntityModelUserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Data_layer.Context.OrderDetail", b =>
                {
                    b.Property<Guid>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AmountTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Contents")
                        .HasColumnType("int");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("Data_layer.Context.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("CratePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("CrateQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InstokeOfCrate")
                        .HasColumnType("int");

                    b.Property<int?>("InstokeOfPiece")
                        .HasColumnType("int");

                    b.Property<bool>("IsPopular")
                        .HasColumnType("bit");

                    b.Property<decimal?>("Kilo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PiecePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Data_layer.Context.ProductImageEnityModel", b =>
                {
                    b.Property<Guid>("ProductImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProductImageId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("Data_layer.Context.ProductWishlist", b =>
                {
                    b.Property<Guid>("ProductWishlistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WishlistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProductWishlistId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WishlistId");

                    b.ToTable("ProductWishlist");
                });

            modelBuilder.Entity("Data_layer.Context.Data.CartEnityModel", b =>
                {
                    b.HasOne("Data_layer.Context.Data.UserRegistrationEntityModel", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data_layer.Context.Data.UserRegistrationEntityModel", b =>
                {
                    b.OwnsOne("Data_layer.Context.Data.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("AddressId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Residence")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AddressId");

                            b1.ToTable("UserRegistration");

                            b1.WithOwner()
                                .HasForeignKey("AddressId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Data_layer.Context.Data.WishlistEntityModel", b =>
                {
                    b.HasOne("Data_layer.Context.Data.UserRegistrationEntityModel", "User")
                        .WithMany("Wishlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data_layer.Context.Order", b =>
                {
                    b.HasOne("Data_layer.Context.CustomerEntityModel", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_layer.Context.Data.UserRegistrationEntityModel", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserRegistrationEntityModelUserId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Data_layer.Context.OrderDetail", b =>
                {
                    b.HasOne("Data_layer.Context.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_layer.Context.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data_layer.Context.Product", b =>
                {
                    b.HasOne("Data_layer.Context.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data_layer.Context.ProductImageEnityModel", b =>
                {
                    b.HasOne("Data_layer.Context.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data_layer.Context.ProductWishlist", b =>
                {
                    b.HasOne("Data_layer.Context.Product", "Product")
                        .WithMany("ProductWishlists")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_layer.Context.Data.WishlistEntityModel", "Wishlist")
                        .WithMany("ProductWishlists")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("Data_layer.Context.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Data_layer.Context.CustomerEntityModel", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Data_layer.Context.Data.UserRegistrationEntityModel", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("Data_layer.Context.Data.WishlistEntityModel", b =>
                {
                    b.Navigation("ProductWishlists");
                });

            modelBuilder.Entity("Data_layer.Context.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Data_layer.Context.Product", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductWishlists");
                });
#pragma warning restore 612, 618
        }
    }
}

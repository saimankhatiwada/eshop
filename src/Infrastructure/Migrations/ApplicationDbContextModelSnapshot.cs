﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("description");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("image_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Domain.Reviews.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_reviews_product_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("first_name");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_id");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("image_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("IdentityId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_identity_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json")
                        .HasColumnName("content");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime>("OccuredOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occured_on_utc");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_on_utc");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("Domain.Products.Product", b =>
                {
                    b.OwnsOne("Domain.Shared.Money", "Money", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("money_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("money_currency");

                            b1.HasKey("ProductId");

                            b1.ToTable("products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId")
                                .HasConstraintName("fk_products_products_id");
                        });

                    b.Navigation("Money")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Reviews.Review", b =>
                {
                    b.HasOne("Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_products_product_id1");

                    b.HasOne("Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_user_user_temp_id");
                });
#pragma warning restore 612, 618
        }
    }
}

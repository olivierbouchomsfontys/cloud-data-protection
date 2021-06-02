﻿// <auto-generated />
using System;
using CloudDataProtection.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CloudDataProtection.Migrations
{
    [DbContext(typeof(AuthenticationDbContext))]
    partial class AuthenticationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CloudDataProtection.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CloudDataProtection.Entities.UserDeletionHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("HashedEmail")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UserDeletionHistory");
                });

            modelBuilder.Entity("CloudDataProtection.Entities.UserDeletionHistoryProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserDeletionHistoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserDeletionHistoryId");

                    b.ToTable("UserDeletionHistoryProgress");
                });

            modelBuilder.Entity("CloudDataProtection.Entities.UserDeletionHistoryProgress", b =>
                {
                    b.HasOne("CloudDataProtection.Entities.UserDeletionHistory", "History")
                        .WithMany("Progress")
                        .HasForeignKey("UserDeletionHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("History");
                });

            modelBuilder.Entity("CloudDataProtection.Entities.UserDeletionHistory", b =>
                {
                    b.Navigation("Progress");
                });
#pragma warning restore 612, 618
        }
    }
}

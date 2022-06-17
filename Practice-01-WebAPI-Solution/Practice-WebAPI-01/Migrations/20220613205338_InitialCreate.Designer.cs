﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Practice_WebAPI_01.Data;

#nullable disable

namespace Practice_WebAPI_01.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220613205338_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Practice_WebAPI_01.Models.HeroUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Credit")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("Practice_WebAPI_01.Models.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("HeroUserId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WeaponTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HeroUserId");

                    b.HasIndex("WeaponTypeId");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("Practice_WebAPI_01.Models.WeaponType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WeaponTypes");
                });

            modelBuilder.Entity("Practice_WebAPI_01.Models.Weapon", b =>
                {
                    b.HasOne("Practice_WebAPI_01.Models.HeroUser", null)
                        .WithMany("Weapons")
                        .HasForeignKey("HeroUserId");

                    b.HasOne("Practice_WebAPI_01.Models.WeaponType", "WeaponType")
                        .WithMany()
                        .HasForeignKey("WeaponTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeaponType");
                });

            modelBuilder.Entity("Practice_WebAPI_01.Models.HeroUser", b =>
                {
                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}

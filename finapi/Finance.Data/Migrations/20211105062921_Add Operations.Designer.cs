﻿// <auto-generated />
using System;
using Finance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Finance.Data.Migrations
{
    [DbContext(typeof(FinApiDbContext))]
    [Migration("20211105062921_Add Operations")]
    partial class AddOperations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("AccountOperationCategory", b =>
                {
                    b.Property<int>("AccountsId")
                        .HasColumnType("int");

                    b.Property<int>("OperationCategoriesId")
                        .HasColumnType("int");

                    b.HasKey("AccountsId", "OperationCategoriesId");

                    b.HasIndex("OperationCategoriesId");

                    b.ToTable("AccountOperationCategory");
                });

            modelBuilder.Entity("AccountUser", b =>
                {
                    b.Property<int>("AccountsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("AccountsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("AccountUser");
                });

            modelBuilder.Entity("Finance.Data.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Finance.Data.Models.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<double>("Ammount")
                        .HasColumnType("double");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("Finance.Data.Models.OperationCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsIncome")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("OperationCategories");
                });

            modelBuilder.Entity("Finance.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Finance.Data.Models.UserLogin", b =>
                {
                    b.Property<string>("Provider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Identifier")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Provider", "Identifier");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("OperationCategoryUser", b =>
                {
                    b.Property<int>("OperationCategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("OperationCategoriesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("OperationCategoryUser");
                });

            modelBuilder.Entity("AccountOperationCategory", b =>
                {
                    b.HasOne("Finance.Data.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance.Data.Models.OperationCategory", null)
                        .WithMany()
                        .HasForeignKey("OperationCategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountUser", b =>
                {
                    b.HasOne("Finance.Data.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Finance.Data.Models.Operation", b =>
                {
                    b.HasOne("Finance.Data.Models.Account", "Account")
                        .WithMany("Operations")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance.Data.Models.User", "Author")
                        .WithMany("Operations")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance.Data.Models.OperationCategory", "Category")
                        .WithMany("Operations")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Account");

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Finance.Data.Models.UserLogin", b =>
                {
                    b.HasOne("Finance.Data.Models.User", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OperationCategoryUser", b =>
                {
                    b.HasOne("Finance.Data.Models.OperationCategory", null)
                        .WithMany()
                        .HasForeignKey("OperationCategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Finance.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Finance.Data.Models.Account", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("Finance.Data.Models.OperationCategory", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("Finance.Data.Models.User", b =>
                {
                    b.Navigation("Operations");

                    b.Navigation("UserLogins");
                });
#pragma warning restore 612, 618
        }
    }
}

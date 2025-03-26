﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Teczter.Data;

#nullable disable

namespace Teczter.Data.Migrations
{
    [DbContext(typeof(TeczterDbContext))]
    [Migration("20250325085955_NotesArray")]
    partial class NotesArray
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Teczter.Domain.Entities.ExecutionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("AssignedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExecutionGroupId")
                        .HasColumnType("int");

                    b.Property<int>("ExecutionState")
                        .HasColumnType("int");

                    b.Property<int?>("FailedStepId")
                        .HasColumnType("int");

                    b.Property<string>("FailureReason")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.PrimitiveCollection<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RevisedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("RevisedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<int?>("TestedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("ExecutionGroupId");

                    b.HasIndex("FailedStepId");

                    b.HasIndex("TestId");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.ExecutionGroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ClosedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExecutionGroupName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.PrimitiveCollection<string>("ExecutionGroupNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("RevisedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("RevisedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<string>("SoftwareVersionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionGroupName")
                        .IsUnique();

                    b.ToTable("ExecutionGroups");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("OwningDepartment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RevisedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("RevisedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.PrimitiveCollection<string>("Urls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.TestStepEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("RevisedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("RevisedOn")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<int>("StepPlacement")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("Urls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestSteps");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<int>("Department")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.ExecutionEntity", b =>
                {
                    b.HasOne("Teczter.Domain.Entities.UserEntity", "AssignedUser")
                        .WithMany("AssignedExcutions")
                        .HasForeignKey("AssignedUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Teczter.Domain.Entities.ExecutionGroupEntity", "ExecutionGroup")
                        .WithMany("Executions")
                        .HasForeignKey("ExecutionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Teczter.Domain.Entities.TestStepEntity", "FailedStep")
                        .WithMany()
                        .HasForeignKey("FailedStepId");

                    b.HasOne("Teczter.Domain.Entities.TestEntity", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("ExecutionGroup");

                    b.Navigation("FailedStep");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.TestStepEntity", b =>
                {
                    b.HasOne("Teczter.Domain.Entities.TestEntity", null)
                        .WithMany("TestSteps")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Teczter.Domain.Entities.ExecutionGroupEntity", b =>
                {
                    b.Navigation("Executions");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.TestEntity", b =>
                {
                    b.Navigation("TestSteps");
                });

            modelBuilder.Entity("Teczter.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("AssignedExcutions");
                });
#pragma warning restore 612, 618
        }
    }
}

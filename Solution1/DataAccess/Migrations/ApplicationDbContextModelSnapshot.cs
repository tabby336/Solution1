﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataAccess;

namespace DataAccess.Migrations
{
    [DbContext(typeof(PlatformManagement))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:.uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("DataAccess.Models.Mark", b =>
                {
                    b.Property<Guid>("ModuleId");

                    b.Property<Guid>("UserId");

                    b.Property<Guid>("Id");

                    b.Property<string>("Notes");

                    b.Property<DateTime>("Timestamp");

                    b.Property<float>("Value");

                    b.HasKey("ModuleId", "UserId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("DataAccess.Repositories.ModuleManagement.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourseId");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<bool>("HasHomework");

                    b.Property<bool>("HasTest");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("UrlPdf")
                        .HasAnnotation("MaxLength", 1024);

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });
        }
    }
}

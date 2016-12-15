using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataAccess;

namespace DataAccess.Migrations
{
    [DbContext(typeof(PlatformManagement))]
    [Migration("20161214205041_DatabaseSetup")]
    partial class DatabaseSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:.uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("DataAccess.Models.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataLink")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<string>("HashTag")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("PhotoUrl")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

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

            modelBuilder.Entity("DataAccess.Models.Module", b =>
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

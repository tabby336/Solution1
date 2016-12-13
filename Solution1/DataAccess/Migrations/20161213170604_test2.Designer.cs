using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Services.Data;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161213170604_test2")]
    partial class test2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:.uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("DataAccess.Mark", b =>
                {
                    b.Property<Guid>("HomeworkId");

                    b.Property<Guid>("StudentId");

                    b.Property<string>("Review");

                    b.Property<float>("Value");

                    b.HasKey("HomeworkId", "StudentId");

                    b.ToTable("Marks");
                });
        }
    }
}

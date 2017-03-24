using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BeltS.Models;

namespace BeltS.Migrations
{
    [DbContext(typeof(BeltSContext))]
    [Migration("20170324174038_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("BeltS.Models.Connection", b =>
                {
                    b.Property<int>("ConnecId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("CurUserId");

                    b.Property<int>("NetUserId");

                    b.Property<string>("Status");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int?>("UserId");

                    b.HasKey("ConnecId");

                    b.HasIndex("UserId");

                    b.ToTable("Connection");
                });

            modelBuilder.Entity("BeltS.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BeltS.Models.Connection", b =>
                {
                    b.HasOne("BeltS.Models.User")
                        .WithMany("Connections")
                        .HasForeignKey("UserId");
                });
        }
    }
}

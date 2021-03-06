﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pumps.Database;

namespace pumps.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190228062414_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("pumps.Models.Pump", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Ampers");

                    b.Property<string>("Name");

                    b.Property<float>("Pressure");

                    b.Property<float>("Temperature");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("Vibration");

                    b.Property<float>("Volume");

                    b.HasKey("Id");

                    b.ToTable("Pumps");
                });

            modelBuilder.Entity("pumps.Models.SensorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int?>("PumpId");

                    b.Property<int>("Sensor");

                    b.Property<float>("Value");

                    b.HasKey("Id");

                    b.HasIndex("PumpId");

                    b.ToTable("SensorLogs");
                });

            modelBuilder.Entity("pumps.Models.SensorLog", b =>
                {
                    b.HasOne("pumps.Models.Pump", "Pump")
                        .WithMany()
                        .HasForeignKey("PumpId");
                });
#pragma warning restore 612, 618
        }
    }
}

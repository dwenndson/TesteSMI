﻿// <auto-generated />
using System;
using IogServices.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IogServices.Migrations
{
    [DbContext(typeof(IogContext))]
    [Migration("20200309145736_MeterUnregistereds")]
    partial class MeterUnregistereds
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IogServices.Models.DAO.CommandTicket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Answer");

                    b.Property<int>("Attempt");

                    b.Property<int>("Command");

                    b.Property<Guid>("CommandId");

                    b.Property<int>("CommunicationStatus");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("FinishDate");

                    b.Property<DateTime>("InitialDate");

                    b.Property<string>("Pdu");

                    b.Property<int>("Status");

                    b.Property<int>("StatusCommand");

                    b.Property<Guid?>("TicketId");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("CommandTickets");
                });

            modelBuilder.Entity("IogServices.Models.DAO.DeviceLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("Date");

                    b.Property<string>("DeviceSerial");

                    b.Property<int>("LogLevel");

                    b.Property<string>("Message");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("DeviceLogs");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Meter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountantStatus");

                    b.Property<bool>("Active");

                    b.Property<decimal>("BillingConstant");

                    b.Property<bool>("Comissioned");

                    b.Property<int>("ConnectionPhase");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Identifier");

                    b.Property<string>("Installation");

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<Guid?>("MeterKeysId");

                    b.Property<Guid?>("MeterModelId");

                    b.Property<Guid?>("ModemId");

                    b.Property<int>("Phase");

                    b.Property<string>("Prefix");

                    b.Property<Guid?>("RateTypeId");

                    b.Property<string>("Registrars");

                    b.Property<string>("Serial")
                        .IsRequired();

                    b.Property<Guid?>("SmcId");

                    b.Property<string>("TcRatio");

                    b.Property<string>("Tli");

                    b.Property<string>("TpRatio");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Serial");

                    b.HasIndex("MeterKeysId");

                    b.HasIndex("MeterModelId");

                    b.HasIndex("ModemId");

                    b.HasIndex("RateTypeId");

                    b.HasIndex("SmcId");

                    b.ToTable("Meters");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterAlarm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<Guid?>("MeterId");

                    b.Property<DateTime>("ReadDateTime");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("MeterId");

                    b.ToTable("MeterAlarms");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterEnergy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DirectEnergy");

                    b.Property<Guid?>("MeterId");

                    b.Property<DateTime>("ReadingTime");

                    b.Property<string>("ReverseEnergy");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("MeterId");

                    b.ToTable("MeterEnergies");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterKeys", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Ak")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Ek")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Mk")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Serial")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Serial");

                    b.ToTable("MeterKeys");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid?>("ManufacturerId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("MeterModels");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterUnregistered", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceEui");

                    b.Property<string>("MeterPhase");

                    b.Property<string>("Serial");

                    b.Property<Guid?>("SmcUnregisteredId");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("SmcUnregisteredId");

                    b.ToTable("MeterUnregistereds");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Modem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceEui")
                        .IsRequired();

                    b.Property<int>("FirmwareVersion");

                    b.Property<int>("Serial");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("DeviceEui");

                    b.ToTable("Modems");
                });

            modelBuilder.Entity("IogServices.Models.DAO.RateType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("RateTypes");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Smc", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("Comissioned");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<Guid?>("ModemId");

                    b.Property<string>("Serial")
                        .IsRequired();

                    b.Property<Guid?>("SmcModelId");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Serial");

                    b.HasIndex("ModemId");

                    b.HasIndex("SmcModelId");

                    b.ToTable("Smcs");
                });

            modelBuilder.Entity("IogServices.Models.DAO.SmcAlarm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ReadDateTime");

                    b.Property<Guid?>("SmcId");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("SmcId");

                    b.ToTable("SmcAlarms");
                });

            modelBuilder.Entity("IogServices.Models.DAO.SmcModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid?>("ManufacturerId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PositionsCount");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("SmcModels");
                });

            modelBuilder.Entity("IogServices.Models.DAO.SmcUnregistered", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceEui");

                    b.Property<string>("Serial");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("SmcUnregistereds");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("CommandType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("FinishDate");

                    b.Property<DateTime>("InitialDate");

                    b.Property<int>("Interval");

                    b.Property<string>("Serial");

                    b.Property<int>("Status");

                    b.Property<int>("StatusCommand");

                    b.Property<Guid>("TicketId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("IogServices.Models.DAO.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("ClientType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Salt");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("IogServices.Models.DAO.CommandTicket", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Ticket", "Ticket")
                        .WithMany("CommandTickets")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Meter", b =>
                {
                    b.HasOne("IogServices.Models.DAO.MeterKeys", "MeterKeys")
                        .WithMany()
                        .HasForeignKey("MeterKeysId");

                    b.HasOne("IogServices.Models.DAO.MeterModel", "MeterModel")
                        .WithMany("Meters")
                        .HasForeignKey("MeterModelId");

                    b.HasOne("IogServices.Models.DAO.Modem", "Modem")
                        .WithMany()
                        .HasForeignKey("ModemId");

                    b.HasOne("IogServices.Models.DAO.RateType", "RateType")
                        .WithMany("Meters")
                        .HasForeignKey("RateTypeId");

                    b.HasOne("IogServices.Models.DAO.Smc", "Smc")
                        .WithMany("Meters")
                        .HasForeignKey("SmcId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterAlarm", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Meter", "Meter")
                        .WithMany("MeterAlarms")
                        .HasForeignKey("MeterId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterEnergy", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Meter", "Meter")
                        .WithMany("Energies")
                        .HasForeignKey("MeterId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterModel", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Manufacturer", "Manufacturer")
                        .WithMany("MeterModels")
                        .HasForeignKey("ManufacturerId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.MeterUnregistered", b =>
                {
                    b.HasOne("IogServices.Models.DAO.SmcUnregistered")
                        .WithMany("Meters")
                        .HasForeignKey("SmcUnregisteredId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.Smc", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Modem", "Modem")
                        .WithMany()
                        .HasForeignKey("ModemId");

                    b.HasOne("IogServices.Models.DAO.SmcModel", "SmcModel")
                        .WithMany("Smcs")
                        .HasForeignKey("SmcModelId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.SmcAlarm", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Smc", "Smc")
                        .WithMany("SmcAlarms")
                        .HasForeignKey("SmcId");
                });

            modelBuilder.Entity("IogServices.Models.DAO.SmcModel", b =>
                {
                    b.HasOne("IogServices.Models.DAO.Manufacturer", "Manufacturer")
                        .WithMany("SmcModels")
                        .HasForeignKey("ManufacturerId");
                });
#pragma warning restore 612, 618
        }
    }
}

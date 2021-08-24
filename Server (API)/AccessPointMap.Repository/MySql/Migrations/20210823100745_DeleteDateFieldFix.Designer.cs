﻿// <auto-generated />
using System;
using AccessPointMap.Repository.MySql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccessPointMap.Repository.MySql.Migrations
{
    [DbContext(typeof(AccessPointMapMySqlDbContext))]
    [Migration("20210823100745_DeleteDateFieldFix")]
    partial class DeleteDateFieldFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("AccessPointMap.Domain.AccessPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Bssid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceType")
                        .HasColumnType("longtext");

                    b.Property<bool>("Display")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Fingerprint")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Frequency")
                        .HasColumnType("double");

                    b.Property<string>("FullSecurityData")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsHidden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsSecure")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Manufacturer")
                        .HasColumnType("longtext");

                    b.Property<bool>("MasterGroup")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<double>("MaxSignalLatitude")
                        .HasColumnType("double");

                    b.Property<int>("MaxSignalLevel")
                        .HasColumnType("int");

                    b.Property<double>("MaxSignalLongitude")
                        .HasColumnType("double");

                    b.Property<double>("MinSignalLatitude")
                        .HasColumnType("double");

                    b.Property<int>("MinSignalLevel")
                        .HasColumnType("int");

                    b.Property<double>("MinSignalLongitude")
                        .HasColumnType("double");

                    b.Property<string>("Note")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("");

                    b.Property<string>("SerializedSecurityData")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("SignalArea")
                        .HasColumnType("double");

                    b.Property<double>("SignalRadius")
                        .HasColumnType("double");

                    b.Property<string>("Ssid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("UserAddedId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserModifiedId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserAddedId");

                    b.HasIndex("UserModifiedId");

                    b.ToTable("AccessPoints");
                });

            modelBuilder.Entity("AccessPointMap.Domain.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedByIp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRevoked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RevokedByIp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("AccessPointMap.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("AdminPermission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsActivated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastLoginDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("LastLoginIp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("");

                    b.Property<bool>("ModPermission")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccessPointMap.Domain.AccessPoint", b =>
                {
                    b.HasOne("AccessPointMap.Domain.User", "UserAdded")
                        .WithMany("AddedAccessPoints")
                        .HasForeignKey("UserAddedId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("AccessPointMap.Domain.User", "UserModified")
                        .WithMany("ModifiedAccessPoints")
                        .HasForeignKey("UserModifiedId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("UserAdded");

                    b.Navigation("UserModified");
                });

            modelBuilder.Entity("AccessPointMap.Domain.RefreshToken", b =>
                {
                    b.HasOne("AccessPointMap.Domain.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("AccessPointMap.Domain.User", b =>
                {
                    b.Navigation("AddedAccessPoints");

                    b.Navigation("ModifiedAccessPoints");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
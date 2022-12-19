﻿// <auto-generated />
using System;
using AccessPointMap.Infrastructure.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccessPointMap.Infrastructure.Sqlite.Migrations
{
    [DbContext(typeof(AccessPointMapDbContext))]
    [Migration("20221219212122_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.12");

            modelBuilder.Entity("AccessPointMap.Domain.AccessPoints.AccessPoint", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AccessPoints");
                });

            modelBuilder.Entity("AccessPointMap.Domain.Identities.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("AccessPointMap.Domain.AccessPoints.AccessPoint", b =>
                {
                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointContributorId", "ContributorId", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointCreationTimestamp", "CreationTimestamp", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointDeviceType", "DeviceType", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointFrequency", "Frequency", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("Value")
                                .HasColumnType("REAL");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPositioning", "Positioning", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("HighSignalLatitude")
                                .HasColumnType("REAL");

                            b1.Property<int>("HighSignalLevel")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("HighSignalLongitude")
                                .HasColumnType("REAL");

                            b1.Property<double>("LowSignalLatitude")
                                .HasColumnType("REAL");

                            b1.Property<int>("LowSignalLevel")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("LowSignalLongitude")
                                .HasColumnType("REAL");

                            b1.Property<double>("SignalArea")
                                .HasColumnType("REAL");

                            b1.Property<double>("SignalRadius")
                                .HasColumnType("REAL");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointRunIdentifier", "RunIdentifier", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointSecurity", "Security", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsSecure")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("RawSecurityPayload")
                                .HasColumnType("TEXT");

                            b1.Property<string>("SecurityProtocols")
                                .HasColumnType("TEXT");

                            b1.Property<string>("SecurityStandards")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointSsid", "Ssid", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointBssid", "Bssid", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointDisplayStatus", "DisplayStatus", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointManufacturer", "Manufacturer", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointNote", "Note", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPresence", "Presence", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointVersionTimestamp", "VersionTimestamp", b1 =>
                        {
                            b1.Property<Guid>("AccessPointId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("AccessPointId");

                            b1.ToTable("AccessPoints");

                            b1.WithOwner()
                                .HasForeignKey("AccessPointId");
                        });

                    b.OwnsMany("AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations.AccessPointAdnnotation", "Adnnotations", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("accesspointId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("accesspointId");

                            b1.ToTable("AccessPointAdnnotation");

                            b1.WithOwner()
                                .HasForeignKey("accesspointId");

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations.AccessPointAdnnotationContent", "Content", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointAdnnotationId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointAdnnotationId");

                                    b2.ToTable("AccessPointAdnnotation");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointAdnnotationId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations.AccessPointAdnnotationTimestamp", "Timestamp", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointAdnnotationId")
                                        .HasColumnType("TEXT");

                                    b2.Property<DateTime>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointAdnnotationId");

                                    b2.ToTable("AccessPointAdnnotation");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointAdnnotationId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations.AccessPointAdnnotationTitle", "Title", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointAdnnotationId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointAdnnotationId");

                                    b2.ToTable("AccessPointAdnnotation");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointAdnnotationId");
                                });

                            b1.Navigation("Content")
                                .IsRequired();

                            b1.Navigation("Timestamp")
                                .IsRequired();

                            b1.Navigation("Title")
                                .IsRequired();
                        });

                    b.OwnsMany("AccessPointMap.Domain.AccessPoints.AccessPointPackets.AccessPointPacket", "Packets", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("accesspointId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("accesspointId");

                            b1.ToTable("AccessPointPacket");

                            b1.WithOwner()
                                .HasForeignKey("accesspointId");

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPackets.AccessPointPacketData", "Data", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointPacketId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointPacketId");

                                    b2.ToTable("AccessPointPacket");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointPacketId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPackets.AccessPointPacketDestinationAddress", "DestinationAddress", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointPacketId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointPacketId");

                                    b2.ToTable("AccessPointPacket");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointPacketId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPackets.AccessPointPacketFrameType", "FrameType", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointPacketId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointPacketId");

                                    b2.ToTable("AccessPointPacket");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointPacketId");
                                });

                            b1.Navigation("Data")
                                .IsRequired();

                            b1.Navigation("DestinationAddress")
                                .IsRequired();

                            b1.Navigation("FrameType")
                                .IsRequired();
                        });

                    b.OwnsMany("AccessPointMap.Domain.AccessPoints.AccessPointStamps.AccessPointStamp", "Stamps", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("accesspointId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("accesspointId");

                            b1.ToTable("AccessPointStamp");

                            b1.WithOwner()
                                .HasForeignKey("accesspointId");

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointContributorId", "ContributorId", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<Guid>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointCreationTimestamp", "CreationTimestamp", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<DateTime>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointDeviceType", "DeviceType", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointFrequency", "Frequency", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<double>("Value")
                                        .HasColumnType("REAL");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointPositioning", "Positioning", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<double>("HighSignalLatitude")
                                        .HasColumnType("REAL");

                                    b2.Property<int>("HighSignalLevel")
                                        .HasColumnType("INTEGER");

                                    b2.Property<double>("HighSignalLongitude")
                                        .HasColumnType("REAL");

                                    b2.Property<double>("LowSignalLatitude")
                                        .HasColumnType("REAL");

                                    b2.Property<int>("LowSignalLevel")
                                        .HasColumnType("INTEGER");

                                    b2.Property<double>("LowSignalLongitude")
                                        .HasColumnType("REAL");

                                    b2.Property<double>("SignalArea")
                                        .HasColumnType("REAL");

                                    b2.Property<double>("SignalRadius")
                                        .HasColumnType("REAL");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointRunIdentifier", "RunIdentifier", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<Guid?>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointSecurity", "Security", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<bool>("IsSecure")
                                        .HasColumnType("INTEGER");

                                    b2.Property<string>("RawSecurityPayload")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("SecurityProtocols")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("SecurityStandards")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointSsid", "Ssid", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Value")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.OwnsOne("AccessPointMap.Domain.AccessPoints.AccessPointStamps.AccessPointStampStatus", "Status", b2 =>
                                {
                                    b2.Property<Guid>("AccessPointStampId")
                                        .HasColumnType("TEXT");

                                    b2.Property<bool>("Value")
                                        .HasColumnType("INTEGER");

                                    b2.HasKey("AccessPointStampId");

                                    b2.ToTable("AccessPointStamp");

                                    b2.WithOwner()
                                        .HasForeignKey("AccessPointStampId");
                                });

                            b1.Navigation("ContributorId")
                                .IsRequired();

                            b1.Navigation("CreationTimestamp")
                                .IsRequired();

                            b1.Navigation("DeviceType")
                                .IsRequired();

                            b1.Navigation("Frequency")
                                .IsRequired();

                            b1.Navigation("Positioning")
                                .IsRequired();

                            b1.Navigation("RunIdentifier")
                                .IsRequired();

                            b1.Navigation("Security")
                                .IsRequired();

                            b1.Navigation("Ssid")
                                .IsRequired();

                            b1.Navigation("Status")
                                .IsRequired();
                        });

                    b.Navigation("Adnnotations");

                    b.Navigation("Bssid")
                        .IsRequired();

                    b.Navigation("ContributorId")
                        .IsRequired();

                    b.Navigation("CreationTimestamp")
                        .IsRequired();

                    b.Navigation("DeviceType")
                        .IsRequired();

                    b.Navigation("DisplayStatus")
                        .IsRequired();

                    b.Navigation("Frequency")
                        .IsRequired();

                    b.Navigation("Manufacturer")
                        .IsRequired();

                    b.Navigation("Note")
                        .IsRequired();

                    b.Navigation("Packets");

                    b.Navigation("Positioning")
                        .IsRequired();

                    b.Navigation("Presence")
                        .IsRequired();

                    b.Navigation("RunIdentifier")
                        .IsRequired();

                    b.Navigation("Security")
                        .IsRequired();

                    b.Navigation("Ssid")
                        .IsRequired();

                    b.Navigation("Stamps");

                    b.Navigation("VersionTimestamp")
                        .IsRequired();
                });

            modelBuilder.Entity("AccessPointMap.Domain.Identities.Identity", b =>
                {
                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityActivation", "Activation", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityEmail", "Email", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("IdentityId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityLastLogin", "LastLogin", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("Date")
                                .HasColumnType("TEXT");

                            b1.Property<string>("IpAddress")
                                .HasColumnType("TEXT");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityName", "Name", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasMaxLength(40)
                                .HasColumnType("TEXT");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityPasswordHash", "PasswordHash", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsOne("AccessPointMap.Domain.Identities.IdentityRole", "Role", b1 =>
                        {
                            b1.Property<Guid>("IdentityId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("IdentityId");

                            b1.ToTable("Identities");

                            b1.WithOwner()
                                .HasForeignKey("IdentityId");
                        });

                    b.OwnsMany("AccessPointMap.Domain.Identities.Tokens.Token", "Tokens", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<string>("CreatedByIpAddress")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsRevoked")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("ReplacedByTokenHash")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("TEXT");

                            b1.Property<string>("RevokedByIpAddress")
                                .HasColumnType("TEXT");

                            b1.Property<string>("TokenHash")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("identityId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("TokenHash")
                                .IsUnique();

                            b1.HasIndex("identityId");

                            b1.ToTable("Token");

                            b1.WithOwner()
                                .HasForeignKey("identityId");
                        });

                    b.Navigation("Activation")
                        .IsRequired();

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("LastLogin")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("PasswordHash")
                        .IsRequired();

                    b.Navigation("Role")
                        .IsRequired();

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using FLServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Migrations
{
    [DbContext(typeof(FLDBContext))]
    [Migration("20190528171438_2019528_12")]
    partial class _2019528_12
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FLServer.Models.Ability", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CastTime");

                    b.Property<int?>("CharacterId1");

                    b.Property<int>("Cooldown");

                    b.Property<int>("Cost");

                    b.Property<int>("Damage");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectileSpeed");

                    b.Property<int>("Range");

                    b.HasKey("CharacterId");

                    b.HasIndex("CharacterId1");

                    b.ToTable("Ability");
                });

            modelBuilder.Entity("FLServer.Models.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttackSpeed");

                    b.Property<int>("Avatar");

                    b.Property<int>("Damage");

                    b.Property<int>("Defense");

                    b.Property<string>("Description");

                    b.Property<int>("MovementSpeed");

                    b.Property<string>("Name");

                    b.Property<int>("PremiumPrice");

                    b.Property<int>("Price");

                    b.Property<int>("Range");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("Size");

                    b.Property<string>("UnderTitle");

                    b.Property<int>("Weight");

                    b.HasKey("CharacterId");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Character");
                });

            modelBuilder.Entity("FLServer.Models.Gamemode", b =>
                {
                    b.Property<int>("GamemodeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("GamemodeId");

                    b.ToTable("Gamemode");
                });

            modelBuilder.Entity("FLServer.Models.Map", b =>
                {
                    b.Property<int>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Image");

                    b.Property<string>("Name");

                    b.HasKey("MapId");

                    b.ToTable("Map");
                });

            modelBuilder.Entity("FLServer.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GamemodeId");

                    b.Property<int?>("MapId");

                    b.Property<DateTime>("MatchPlayed");

                    b.Property<int>("MatchTime");

                    b.Property<int?>("UserId");

                    b.Property<int>("Winner");

                    b.HasKey("MatchId");

                    b.HasIndex("GamemodeId");

                    b.HasIndex("MapId");

                    b.HasIndex("UserId");

                    b.ToTable("Match");
                });

            modelBuilder.Entity("FLServer.Models.Passive", b =>
                {
                    b.Property<int>("PassiveId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacterFK");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("PassiveId");

                    b.HasIndex("CharacterFK")
                        .IsUnique();

                    b.ToTable("Passive");
                });

            modelBuilder.Entity("FLServer.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacterId");

                    b.Property<int>("StatsId");

                    b.Property<int?>("TeamId");

                    b.Property<int>("UserId");

                    b.HasKey("PlayerId");

                    b.HasIndex("CharacterId");

                    b.HasIndex("StatsId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("FLServer.Models.Purchase", b =>
                {
                    b.Property<int>("PurchaseId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("PurchaseDate");

                    b.HasKey("PurchaseId");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("FLServer.Models.ServerVersion", b =>
                {
                    b.Property<string>("VersionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("VersionNr");

                    b.HasKey("VersionId");

                    b.ToTable("ServerVersion");
                });

            modelBuilder.Entity("FLServer.Models.Stats", b =>
                {
                    b.Property<int>("StatsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DamageDealt");

                    b.Property<int>("DamageTaken");

                    b.Property<int>("Deaths");

                    b.Property<int>("HighestPercentage");

                    b.Property<int>("Kills");

                    b.HasKey("StatsId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("FLServer.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MatchId");

                    b.HasKey("TeamId");

                    b.HasIndex("MatchId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("FLServer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Avatar");

                    b.Property<int>("Balance");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("Exp");

                    b.Property<DateTime>("LastOnline");

                    b.Property<int>("Level");

                    b.Property<int>("NormalElo");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("PremiumBalance");

                    b.Property<int>("RankedElo");

                    b.Property<string>("Status");

                    b.Property<string>("UniqueIdentifier");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<bool>("Verified");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("FLServer.Models.UserFriend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FriendId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserFriend");
                });

            modelBuilder.Entity("FLServer.Models.Ability", b =>
                {
                    b.HasOne("FLServer.Models.Character")
                        .WithMany("Abilities")
                        .HasForeignKey("CharacterId1");
                });

            modelBuilder.Entity("FLServer.Models.Match", b =>
                {
                    b.HasOne("FLServer.Models.Gamemode", "Gamemode")
                        .WithMany()
                        .HasForeignKey("GamemodeId");

                    b.HasOne("FLServer.Models.Map", "Map")
                        .WithMany()
                        .HasForeignKey("MapId");

                    b.HasOne("FLServer.Models.User")
                        .WithMany("Matches")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FLServer.Models.Passive", b =>
                {
                    b.HasOne("FLServer.Models.Character", "Character")
                        .WithOne("Passive")
                        .HasForeignKey("FLServer.Models.Passive", "CharacterFK")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FLServer.Models.Player", b =>
                {
                    b.HasOne("FLServer.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FLServer.Models.Stats", "Stats")
                        .WithMany()
                        .HasForeignKey("StatsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FLServer.Models.Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.HasOne("FLServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FLServer.Models.Team", b =>
                {
                    b.HasOne("FLServer.Models.Match")
                        .WithMany("Teams")
                        .HasForeignKey("MatchId");
                });
#pragma warning restore 612, 618
        }
    }
}

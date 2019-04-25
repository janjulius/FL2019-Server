﻿// <auto-generated />
using System;
using FLServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FLServer.Migrations
{
    [DbContext(typeof(FLDBContext))]
    partial class FLDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FLServer.Models.Ability", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CastTime");

                    b.Property<int>("Cooldown");

                    b.Property<int>("Cost");

                    b.Property<int>("Damage");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectileSpeed");

                    b.Property<int>("Range");

                    b.HasKey("CharacterId");

                    b.ToTable("Ability");
                });

            modelBuilder.Entity("FLServer.Models.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Character");
                });

            modelBuilder.Entity("FLServer.Models.Gamemode", b =>
                {
                    b.Property<int>("GamemodeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("GamemodeId");

                    b.ToTable("Gamemode");
                });

            modelBuilder.Entity("FLServer.Models.Map", b =>
                {
                    b.Property<int>("MapId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("Image");

                    b.Property<string>("Name");

                    b.HasKey("MapId");

                    b.ToTable("Map");
                });

            modelBuilder.Entity("FLServer.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GameMode");

                    b.Property<int>("Map");

                    b.Property<DateTime>("MatchPlayed");

                    b.Property<int>("MatchTime");

                    b.Property<int>("Winner");

                    b.HasKey("MatchId");

                    b.ToTable("Match");
                });

            modelBuilder.Entity("FLServer.Models.Passive", b =>
                {
                    b.Property<int>("PassiveId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("PassiveId");

                    b.ToTable("Passive");
                });

            modelBuilder.Entity("FLServer.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("PlayerId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("FLServer.Models.Purchase", b =>
                {
                    b.Property<int>("PurchaseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("PurchaseDate");

                    b.HasKey("PurchaseId");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("FLServer.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("TeamId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("FLServer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Avatar");

                    b.Property<int>("Balance");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Elo");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("Exp");

                    b.Property<DateTime>("LastOnline");

                    b.Property<int>("Level");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("PremiumBalance");

                    b.Property<string>("Status");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<bool>("Verified");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FLServer.Models.UserFriend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FriendId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserFriend");
                });
#pragma warning restore 612, 618
        }
    }
}
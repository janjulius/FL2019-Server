﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FLServer.Models
{
    public partial class FLDBContext : DbContext
    {
        public FLDBContext()
        {
        }

        public FLDBContext(DbContextOptions<FLDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFriend> UserFriend { get; set; }
        public virtual DbSet<Ability> Ability { get; set; }
        public virtual DbSet<Character> Character { get; set; }
        public virtual DbSet<Gamemode> Gamemode { get; set; }
        public virtual DbSet<Map> Map { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<Passive> Passive { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<ServerVersion> ServerVersion {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=fl2019.database.windows.net;Database=FLDB;Trusted_Connection=False;Encrypt=True;uid=FLDbLogin;password=P7QrZ)s#xnZTE(q7");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>(entity =>
            {

            });
        }
    }
}
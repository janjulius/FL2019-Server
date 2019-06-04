using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySql.Data.EntityFrameworkCore.Extensions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

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

        //public virtual DbSet<Admin> Admin { get; set; }
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
        public virtual DbSet<Stats> Stats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=78.141.211.160;Database=FLDB;User=FLDblogin;Password=P7QrZ)s#xnZTE(q7;SslMode=Preferred;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>(entity =>
            {

            });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            //modelBuilder.Entity<User>(u =>
            //{
            //    u.Property(p => p.Username).ForMySQLHasCollation("utf8_bin");
            //    u.Property(p => p.Username).ForMySQLHasCharset("utf8");
            //    u.Property(p => p.Status).ForMySQLHasCollation("utf8_bin");
            //    u.Property(p => p.Status).ForMySQLHasCharset("utf8");
            //});

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Character>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Character>().HasIndex(c => c.CharacterId).IsUnique();

            modelBuilder.Entity<Player>().Property(p => p.PlayerId).UseMySqlIdentityColumn();

            
            //modelBuilder.Entity<Player>().HasIndex(c => c.PlayerId).IsUnique();

            //modelBuilder.Entity<Player>().HasOne(p => p.User).WithOne().HasForeignKey<User>(u => u.UserId);
            //modelBuilder.Entity<Player>().HasOne(p => p.Stats).WithOne().HasForeignKey<Stats>(s => s.StatsId);
            //modelBuilder.Entity<Player>().HasOne(p => p.Character).WithOne().HasForeignKey<Character>(s => s.CharacterId);
        }
    }
}

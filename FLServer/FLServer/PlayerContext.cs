using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLServer
{
    class PlayerContext : DbContext
    {
        public DbSet<Usertest> Usertest {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:fl2019.database.windows.net,1433;
Initial Catalog=FLDB;
Persist Security Info=False;
User ID=FLDbLogin;
Password=P7QrZ)s#xnZTE(q7;
MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}

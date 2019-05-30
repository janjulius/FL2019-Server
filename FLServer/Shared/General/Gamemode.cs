using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.General
{
    public static class Gamemode
    {

        public static void Add(string name, string description)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                ctx.Gamemode.Add(new FLServer.Models.Gamemode()
                {
                    Name = name,
                    Description = description
                });
                ctx.SaveChanges();
            }
        }

        public static FLServer.Models.Gamemode GetGamemodeById(int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.Gamemode.Where(gm => gm.GamemodeId == id).FirstOrDefault();
            }
            
        }
    }
}

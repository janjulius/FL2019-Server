using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.General
{
    public static class Map
    {

        public static void Add(string name, string description, int image)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                ctx.Map.Add(new FLServer.Models.Map()
                {
                    Name = name,
                    Description = description,
                    Image = image
                });

                ctx.SaveChanges();
            }
        }

        public static FLServer.Models.Map GetMapById(int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.Map.Where(m => m.MapId == id).FirstOrDefault();
            }
        }
    }
}

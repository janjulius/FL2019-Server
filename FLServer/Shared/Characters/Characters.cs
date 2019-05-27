using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Characters
{
    class Characters
    {

        public void CreateCharacter(
         string name,
         string desc,
         string undertitle,
         int dmg,
         int ms,
         int weight,
         int attackspeed,
         int range,
         int size,
         int def ,
         DateTime releasedate,
         int price,
         int premiumprice)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Character newChar = new Character()
                {
                    Name = name,
                    Description = desc,
                    UnderTitle = undertitle,
                    Damage = dmg,
                    MovementSpeed = ms,
                    Weight = weight,
                    AttackSpeed = attackspeed,
                    Range = range,
                    Size = size,
                    Defense = def,
                    ReleaseDate = releasedate,
                    Price = price,
                    PremiumPrice = premiumprice
                };
                ctx.Character.Add(newChar);
                ctx.SaveChanges();
            }
        }
    }
}

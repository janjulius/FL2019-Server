using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Characters
{
    public static class Characters
    {

        public static void CreateCharacter(
         string name,
         string desc,
         string undertitle,
         int dmg,
         int ms,
         int weight,
         int attackspeed,
         int range,
         int size,
         int def,
         DateTime releasedate,
         int price,
         int premiumprice,
         float heavyCooldown,
         float abilityCooldown)
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
                    PremiumPrice = premiumprice,
                    ReferenceId = ctx.Character.Count() == 0 ? 0 : ctx.Character.Last().ReferenceId + 1,
                    HeavyCoolDown = heavyCooldown,
                    AbilityCoolDown = abilityCooldown
                };
                ctx.Character.Add(newChar);
                ctx.SaveChanges();
            }
        }

        public static Character GetCharacterById(int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.Character.Where(c => c.CharacterId == id).FirstOrDefault();
            }
        }

        public static Character GetCharacterByName(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.Character.Where(c => c.Name == name).FirstOrDefault();
            }
        }

        /// <summary>
        /// do not use
        /// </summary>
        public static void Truncate()
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                foreach(Character c in ctx.Character)
                {
                    ctx.Character.Remove(c);
                }
                ctx.SaveChanges();
            }
        }
    }
}

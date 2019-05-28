using FLServer.Models;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Characters
{
    public static class CharacterMethods
    {
        public static CharacterInfo GetCharacterAsCharacterInfoPacket(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Character c = GetCharacterByName(name);
                CharacterInfo result = new CharacterInfo(c.Name, c.Description, c.UnderTitle, c.Damage, c.MovementSpeed, c.Weight, c.AttackSpeed, c.Range, c.Size, c.Defense);
                return result;
            }
        }
        public static Character GetCharacterByName(string name)
        {
            using(FLDBContext ctx = new FLDBContext())
            {
                Character c = null;
                try
                {
                    c = ctx.Character.Where(n => n.Name == name).First();
                }
                catch
                {
                    return null;
                }
                return c;
            }
        }
    }
}

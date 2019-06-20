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
        public static CharacterInformation GetCharacterAsCharacterInfoPacket(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Character c = GetCharacterByName(name);
                CharacterInformation result = new CharacterInformation(c.Name, c.Description, c.UnderTitle, c.Damage, c.MovementSpeed, c.Weight, c.AttackSpeed, c.Range, c.Size, c.Defense, c.PremiumPrice, c.Price, c.ReferenceId, c.HeavyCoolDown, c.AbilityCoolDown);
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
                    c = ctx.Character.Where(character => character.Name == name).First();
                }
                catch
                {
                    return null;
                }
                return c;
            }
        }

        public static Character GetCharacterByReferenceId(int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Character c = null;
                try
                {
                    c = ctx.Character.Where(character => character.ReferenceId == id).First();
                }
                catch
                {
                    return null;
                }
                return c;
            }
        }

        public static CharacterInformationArray GetAllCharactersAsCharacterInfoPackets()
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                CharacterInformationArray cia = new CharacterInformationArray();
                List<CharacterInformation> chars = new List<CharacterInformation>();
                for(int i = 0; i < Constants.PacketConstants.CharacterCount; i++)
                {
                    chars.Add(CharacterToCharacterInformationPacket(ctx.Character.FirstOrDefault()));
                }
                var realchars = ctx.Character.Where(c => c.Name != "Default Char").AsEnumerable().ToList();
                for (int i = 0; i < realchars.Count(); i++)
                {
                    chars[i] = CharacterToCharacterInformationPacket(realchars[i]);
                }
                cia.chars = chars.ToArray();
                return cia;
            }
        }

        public static CharacterInformation CharacterToCharacterInformationPacket(Character c)
        {
            return new CharacterInformation(c.Name, c.Description, c.UnderTitle, c.Damage, c.MovementSpeed, c.Weight, c.AttackSpeed, c.Range, c.Size, c.Defense, c.PremiumPrice, c.Price, c.ReferenceId, c.HeavyCoolDown, c.AbilityCoolDown);
        }
    }
}

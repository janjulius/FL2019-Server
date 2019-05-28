using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public class CharacterInformation : Packet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnderTitle { get; set; }

        public int Damage { get; set; }
        public int MovementSpeed { get; set; }
        public int Weight { get; set; }
        public int AttackSpeed { get; set; }
        public int Range { get; set; }
        public int Size { get; set; }
        public int Defense { get; set; }

        public CharacterInformation() { }
        public CharacterInformation(string name, string description, string underTitle, int damage, int movementspeed, int weight, int attackspeed, int range, int size, int defense)
        {
            Name = name;
            Description = description;
            UnderTitle = underTitle;
            Damage = damage;
            MovementSpeed = movementspeed;
            Weight = weight;
            AttackSpeed = attackspeed;
            Range = range;
            Size = size;
            Defense = defense;
        }
    }
}

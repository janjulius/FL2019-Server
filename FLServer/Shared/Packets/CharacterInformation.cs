using Shared.Constants;
using System.Runtime.InteropServices;

namespace Shared.Packets
{
    public struct CharacterInformation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.BigStringSize)]
        public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string UnderTitle;

        public int Damage;
        public int MovementSpeed;
        public int Weight;
        public int AttackSpeed;
        public int Range;
        public int Size;
        public int Defense;

        public int PremiumPrice;
        public int RegularPrice;

        public int ReferenceId;

        public float HeavyCoolDown;
        public float AbilityCoolDown;

        public CharacterInformation(string name, string description, string underTitle, int damage,
            int movementspeed, int weight, int attackspeed, int range, int size, int defense, int premiumprice, int regularprice,
            int referenceId, float heavyCoolDown, float abilityCoolDown)
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
            PremiumPrice = premiumprice;
            RegularPrice = regularprice;
            ReferenceId = referenceId;
            HeavyCoolDown = heavyCoolDown;
            AbilityCoolDown = abilityCoolDown;
        }
    }
}

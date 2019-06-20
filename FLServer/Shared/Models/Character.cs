using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FLServer.Models
{
    public partial class Character
    {
        [Key]
        public int CharacterId { get; set; }
        public int ReferenceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnderTitle { get; set; }
        public int Avatar { get; set; }
        public int Damage { get; set; }
        public int MovementSpeed { get; set; }
        public int Weight { get; set; }
        public int AttackSpeed { get; set; }
        public int Range { get; set; }
        public int Size { get; set; }
        public int Defense { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Price { get; set; }
        public int PremiumPrice { get; set; }
        public float HeavyCoolDown { get; set; }
        public float AbilityCoolDown { get; set; }
        public Ability[] Abilities { get; set; }
        public Passive Passive { get; set; }
    }
}

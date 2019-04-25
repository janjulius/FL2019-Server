using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class Ability
    {
        [Key]
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Damage { get; set; }
        public int Range { get; set; }
        public int Cooldown { get; set; }
        public int Cost { get; set; }
        public int CastTime { get; set; }
        public int ProjectileSpeed { get; set; }

    }
}

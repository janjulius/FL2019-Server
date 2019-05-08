using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class CharacterAbility
    {
        [Key]
        public int CharacterAbilityId { get; set; }
        public int CharacterId { get; set; }
        public int AbilityId { get; set; }
        
    }
}

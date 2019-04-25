using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class UserCharacter
    {
        [Key]
        public int UserCharacterId { get; set; }
        public int UserId { get; set; }
        public int CharacterId { get; set; }
        
    }
}

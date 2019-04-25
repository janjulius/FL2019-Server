using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class Gamemode
    {
        [Key]
        public int GamemodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}

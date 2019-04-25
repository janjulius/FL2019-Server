using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class Player
    {
        [Key]
        public int PlayerId { get; set; }
        
    }
}

using System;
using System.Collections.Generic;

namespace FLServer.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Exp { get; set; }
        public int Level { get; set; }
        public DateTime CreationDate { get; set; }
        public int Balance { get; set; }
        public int PremiumBalance { get; set; }
    }
}

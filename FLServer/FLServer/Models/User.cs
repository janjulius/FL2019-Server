using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLServer.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Elo { get; set; }
        public int Exp { get; set; }
        public int Level { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastOnline { get; set; }
        public int Balance { get; set; }
        public int PremiumBalance { get; set; }
        public int Avatar { get; set; }
        public string Status { get; set; }
        public bool Verified { get; set; }
    }
}

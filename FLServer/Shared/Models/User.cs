using Shared.Constants;
using Shared.Encoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLServer.Models
{
    public partial class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string UniqueIdentifier { get; set; }
        public int NormalElo { get; set; }
        public int RankedElo { get; set; }
        public string Rank { get; set; }
        public int Exp { get; set; }
        public int Level { get; set; }
        public int Rights { get; set; } = 0;
        public DateTime CreationDate { get; set; }
        public DateTime LastOnline { get; set; }
        public int Balance { get; set; }
        public int PremiumBalance { get; set; }
        public int Avatar { get; set; }
        public string Status { get; set; }
        public bool Verified { get; set; }
        
        [NotMapped]
        public List<bool> OwnedCharacters
        {
            get { return EFEncoder.DecodeStringToBoolList(OwnedCharactersString); }
            set { OwnedCharactersString = EFEncoder.EncodeBoolListToString(value); }
        }

        public string OwnedCharactersString { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}

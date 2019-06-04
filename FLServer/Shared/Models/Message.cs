using FLServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Shared.Models
{
    public partial class Message
    {
        [Key]
        public int MessageId { get; set; }

        public int SenderId { get; set; }
        public User User1 { get; set; }
        public int ReceiverId { get; set; }
        public User User2 { get; set; }
        public string MessageText { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

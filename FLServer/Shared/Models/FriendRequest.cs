using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLServer.Models
{
    public partial class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestType { get; set; }
    }
}

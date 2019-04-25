using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLServer.Models
{
    public partial class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

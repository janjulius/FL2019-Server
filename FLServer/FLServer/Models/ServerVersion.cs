using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class ServerVersion
    {
        [Key]
        public string VersionId { get; set; }
        public string VersionNr { get; set; }

    }
}

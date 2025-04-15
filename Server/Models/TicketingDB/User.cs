using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ITTicketingProject.Server.Models.TicketingDB
{
    [Table("Users", Schema = "dbo")]
    public partial class User
    {
        [Column("t")]
        [Required]
        public string T { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ITTicketingProject.Server.Models.TicketingDB
{
    [Table("tickets", Schema = "dbo")]
    public partial class Ticket
    {
        [Column("ticketID")]
        [Required]
        public int TicketId { get; set; }

        [Column("affectedUser")]
        public int? AffectedUser { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("owner")]
        [Required]
        public int Owner { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; }
    }
}
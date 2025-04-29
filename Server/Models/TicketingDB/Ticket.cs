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
        [Key]
        [Column("ticketID")]
        [Required]
        public int TicketId { get; set; }

        [Column("affectedUser")]
        [Required]
        public string AffectedUser { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set; }

        [Column("owner")]
        public string Owner { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; }

        [Column("location")]
        public string Location { get; set; }
    }
}
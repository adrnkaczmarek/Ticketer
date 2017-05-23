using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ticketer.Database
{
    public class TicketResponse
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required]
        public string Content { get; set; }

        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User Sender { get; set; }
    }
}

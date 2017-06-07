using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Ticketer.Database.Interfaces;

namespace Ticketer.Database
{
    public class ExternalTicketResponse : ITicketResponse<ExternalClient>
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required]
        public string Content { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        public virtual ExternalClient Sender { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}

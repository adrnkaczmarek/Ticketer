using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Ticketer.Database.Enums;

namespace Ticketer.Database
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
        public TicketState State { get; set; } = TicketState.Open;
        
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        [ForeignKey(nameof(Assigned))]
        public string AssignedId { get; set; }
        [ForeignKey(nameof(AssignedGroup))]
        public int AssignedGroupId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Group AssignedGroup { get; set; }
        public virtual User Assigned { get; set; }
        public virtual ICollection<TicketResponse> TicketResponses { get; set; }
        public virtual ICollection<ExternalTicketResponse> ExternalTicketResponses { get; set; }
    }
}

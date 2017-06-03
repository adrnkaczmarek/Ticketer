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
        [Display(Name = "Created at")]
        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
        public TicketState State { get; set; } = TicketState.Open;
        
        [Display(Name = "Company")]
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        [Display(Name = "Assigned User")]
        [ForeignKey(nameof(Assigned))]
        public string AssignedId { get; set; }
        [Display(Name = "Group")]
        [ForeignKey(nameof(AssignedGroup))]
        public int AssignedGroupId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Group AssignedGroup { get; set; }
        public virtual User Assigned { get; set; }
        public virtual ICollection<TicketResponse> TicketResponses { get; set; }
        public virtual ICollection<ExternalTicketResponse> ExternalTicketResponses { get; set; }
    }
}

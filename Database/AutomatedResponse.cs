using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database.Enums;

namespace Ticketer.Database
{
    public class AutomatedResponse
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Match { get; set; }
        public TicketPriority MaxPriority { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

        [ForeignKey(nameof(CreatedBy)), Required]
        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public virtual ICollection<GroupAutomatedResponse> GroupAutomatedResponses { get; set; }
    }
}

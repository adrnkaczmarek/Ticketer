using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database.Enums;

namespace Ticketer.Models
{
    public class SubmitTicketViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;
        
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}

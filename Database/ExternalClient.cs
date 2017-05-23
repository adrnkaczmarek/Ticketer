using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ticketer.Database
{
    public class ExternalClient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<ExternalTicketResponse> TicketResponses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database;
using Ticketer.Database.Enums;
using Ticketer.Database.Interfaces;

namespace Ticketer.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketState State { get; set; }
        public User Assigned { get; set; }
        public Company Company { get; set; }

        public IEnumerable<ITicketResponse<IUser>> Responses { get; set; }
    }
}

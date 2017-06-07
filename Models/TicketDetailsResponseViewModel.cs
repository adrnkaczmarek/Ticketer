using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database.Interfaces;

namespace Ticketer.Models
{
    public class TicketDetailsResponseViewModel : ITicketResponse<IUser>
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public IUser Sender { get; set; }
    }
}

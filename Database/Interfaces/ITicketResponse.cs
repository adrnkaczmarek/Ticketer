using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketer.Database.Interfaces
{
    public interface ITicketResponse<TUser>
        where TUser : IUser
    {
        DateTime Timestamp { get; set; }
        string Content { get; set; }
        TUser Sender { get; set; }
    }
}

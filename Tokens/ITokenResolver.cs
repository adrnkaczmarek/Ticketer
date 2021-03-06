﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketer.Database;

namespace Ticketer.Tokens
{
    public interface ITokenResolver
    {
        Task<Source> ResolveSourceToken(string encodedToken);
        Task<Ticket> ResolveTicketToken(string encodedToken);
        Task<ExternalClient> ResolveExternalClientToken(string encodedToken);
    }
}

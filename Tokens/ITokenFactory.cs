using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketer.Tokens
{
    public interface ITokenFactory
    {
        Task<JwtSecurityToken> GetSourceToken(int sourceId);
        Task<JwtSecurityToken> GetTicketToken(int ticketId, int externalUserId);
        JwtSecurityToken RecreateToken(JwtSecurityToken token);
        bool ValidateToken(JwtSecurityToken token);
    }
}

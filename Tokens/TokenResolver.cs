using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ticketer.Database;

namespace Ticketer.Tokens
{
    public class TokenResolver : ITokenResolver
    {
        private readonly TicketContext _context;
        private readonly TokenProviderOptions _options;
        private readonly ITokenFactory _tokenFactory;

        public TokenResolver(TicketContext context, TokenProviderOptions options, ITokenFactory factory)
        {
            _context = context;
            _options = options;
            _tokenFactory = factory;
        }

        public async Task<Source> ResolveSourceToken(string encodedToken)
        {
            var token = new JwtSecurityToken(encodedToken);
            if (!IsSigned(token))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var sourceClaim = GetClaim(token, TokenClaims.SourceWebsite);
            var companyIdClaim = GetClaim(token, TokenClaims.CompanyId);
            if (sourceClaim == null || companyIdClaim == null)
            {
                throw new ArgumentException("Source claim or/and company claim are missing.");
            }

            if (!int.TryParse(companyIdClaim.Value, out int companyId))
            {
                throw new ArgumentException("CompanyId has to be of numeric type.");
            }

            var company = await _context.Company
                .Include(c => c.Sources)
                .Include($"{nameof(Company.Sources)}.{nameof(Source.SourceRoutings)}")
                .SingleOrDefaultAsync(c => c.Id == companyId);

            return company?.Sources.SingleOrDefault(c => c.Website == sourceClaim.Value);
        }

        public async Task<Ticket> ResolveTicketToken(string encodedToken)
        {
            var token = new JwtSecurityToken(encodedToken);
            if (!IsSigned(token))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var ticketIdClaim = GetClaim(token, TokenClaims.TicketId);
            if (ticketIdClaim == null)
            {
                throw new ArgumentException("Ticket claim is missing.");
            }

            if (!int.TryParse(ticketIdClaim.Value, out int ticketId))
            {
                throw new ArgumentException("TicketId has to be of numeric type.");
            }

            return await _context.Tickets
                .Include(t => t.ExternalTicketResponses)
                .Include(t => t.TicketResponses)
                .Include($"{nameof(Ticket.ExternalTicketResponses)}.{nameof(ExternalTicketResponse.Sender)}")
                .Include($"{nameof(Ticket.TicketResponses)}.{nameof(TicketResponse.Sender)}")
                .SingleOrDefaultAsync(c => c.Id == ticketId);
        }

        public async Task<ExternalClient> ResolveExternalClientToken(string encodedToken)
        {
            var token = new JwtSecurityToken(encodedToken);
            if (!IsSigned(token))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var clientIdClaim = GetClaim(token, TokenClaims.ExternalUserId);
            if (clientIdClaim == null)
            {
                throw new ArgumentException("Client claim is missing.");
            }

            if (!int.TryParse(clientIdClaim.Value, out int clientId))
            {
                throw new ArgumentException("ClientId has to be of numeric type.");
            }

            return await _context.ExternalClients
                .SingleOrDefaultAsync(c => c.Id == clientId);
        }

        private Claim GetClaim(JwtSecurityToken token, string type)
        {
            return token.Claims.SingleOrDefault(c => c.Type == type);
        }

        private bool IsSigned(JwtSecurityToken token)
        {
            return _tokenFactory.ValidateToken(token);
        }
    }
}

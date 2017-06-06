using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketer.Database;

namespace Ticketer.Tokens
{
    public class TokenResolver : ITokenResolver
    {
        private readonly TicketContext _context;
        private readonly TokenProviderOptions _options;

        public TokenResolver(TicketContext context, TokenProviderOptions options)
        {
            _context = context;
            _options = options;
        }

        public async Task<Source> ResolveSourceToken(string encodedToken)
        {
            var token = new JwtSecurityToken(encodedToken);
            if (!IsSigned(token))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }

            var sourceClaim = token.Claims.SingleOrDefault(c => c.Type == TokenClaims.SourceWebsite);
            var companyIdClaim = token.Claims.SingleOrDefault(c => c.Type == TokenClaims.CompanyId);
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

        private bool IsSigned(JwtSecurityToken token)
        {
            return true;
        }
    }
}

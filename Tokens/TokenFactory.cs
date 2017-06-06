using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ticketer.Database;

namespace Ticketer.Tokens
{
    public class TokenFactory : ITokenFactory
    {
        private readonly TicketContext _context;
        private readonly TokenProviderOptions _options;

        public TokenFactory(TicketContext context, TokenProviderOptions options)
        {
            _context = context;
            _options = options;
        }

        public async Task<JwtSecurityToken> GetSourceToken(int sourceId)
        {
            var source = await _context.Sources.SingleOrDefaultAsync(s => s.Id == sourceId);
            if (source == null)
            {
                return null;
            }

            var claims = new HashSet<Claim>(GetBaseClaims())
            {
                new Claim(TokenClaims.SourceWebsite, source.Website),
                new Claim(TokenClaims.CompanyId, source.CompanyId.ToString())
            };

            return CreateToken(claims);
        }

        private JwtSecurityToken CreateToken(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(
                _options.Issuer, 
                _options.Audience, 
                claims, 
                DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(_options.ExpiresIn), 
                _options.SigningCredentials);
            return jwt;
        }

        private IEnumerable<Claim> GetBaseClaims()
        {
            yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            yield return new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64);
        }

        private static long ToUnixEpochDate(DateTime date)
            =>
                (long)
                Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}

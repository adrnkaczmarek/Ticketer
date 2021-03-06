﻿using System;
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

        public async Task<JwtSecurityToken> GetTicketToken(int ticketId, int externalUserId)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(s => s.Id == ticketId);
            if (ticket == null)
            {
                return null;
            }

            var claims = new HashSet<Claim>(GetBaseClaims())
            {
                new Claim(TokenClaims.TicketId, ticket.Id.ToString()),
                new Claim(TokenClaims.ExternalUserId, externalUserId.ToString())
            };

            return CreateToken(claims);
        }

        public JwtSecurityToken RecreateToken(JwtSecurityToken token)
        {
            return new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                token.Claims.Where(t => !string.Equals(t.Type, JwtRegisteredClaimNames.Aud,
                    StringComparison.Ordinal)),
                token.ValidFrom,
                token.ValidTo,
                _options.SigningCredentials
            );
        }

        public bool ValidateToken(JwtSecurityToken token)
        {
            var newToken = new JwtSecurityToken(RecreateToken(token).Encode());
            return string.Equals(newToken.RawSignature, token.RawSignature, StringComparison.Ordinal);
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

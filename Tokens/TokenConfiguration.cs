using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ticketer.Tokens
{
    public static class TokenConfiguration
    {
        public static IServiceCollection UseTokens(this IServiceCollection services, IConfigurationSection tokenSettings)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings[nameof(TokenProviderOptions.SigningKey)]));
            var tokenOptions = new TokenProviderOptions
            {
                Path = tokenSettings[nameof(TokenProviderOptions.Path)],
                Audience = tokenSettings[nameof(TokenProviderOptions.Audience)],
                Issuer = tokenSettings[nameof(TokenProviderOptions.Issuer)],
                ExpiresIn = Convert.ToInt32(tokenSettings[nameof(TokenProviderOptions.ExpiresIn)]),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            services.AddSingleton(provider => tokenOptions);
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<ITokenResolver, TokenResolver>();

            return services;
        }
    }
}

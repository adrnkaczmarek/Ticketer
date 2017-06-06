using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Ticketer.Tokens
{
    public class TokenProviderOptions
    {
        public string Path { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
        public int ExpiresIn { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}

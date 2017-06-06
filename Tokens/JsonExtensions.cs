using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ticketer.Tokens
{
    public static class JsonExtensions
    {
        public static string ToIndentedJsonString<TObject>(this TObject obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings {Formatting = Formatting.Indented});
        }

        public static string Encode(this JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

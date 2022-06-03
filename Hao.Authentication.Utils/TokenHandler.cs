using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Utils
{
    public class TokenHandler
    {
        public string BuilderToken(TokenMsg info)
        {
            var claims = new List<Claim>();
            if (info.Pairs != null)
            {
                foreach (var p in info.Pairs) { claims.Add(new Claim($"scope{p.Key}", p.Value)); }
            }

            claims.Add(new Claim(ClaimTypes.Sid, info.Id));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, info.Name));
            claims.Add(new Claim(ClaimTypes.Role, info.Role));
            claims.Add(new Claim(ClaimTypes.System, info.System));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKeyBt = Encoding.UTF8.GetBytes(info.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = info.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(6).AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKeyBt), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class TokenMsg
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string System { get; set; }

        public string Key { get; set; }
        public string Issuer { get; set; }
        public Dictionary<string, string> Pairs { get; set; }
    }
}

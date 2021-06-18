using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLearnV1.Security
{
    public class Jwt
    {
        private IConfiguration _config;

        public Jwt(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJWTToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

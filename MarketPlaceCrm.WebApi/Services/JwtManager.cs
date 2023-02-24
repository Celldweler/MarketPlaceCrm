using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlaceCrm.WebApi.Services
{
    public class JwtManager
    {
        private readonly IConfigurationSection _jwtConfig;

        public JwtManager(IConfiguration configuration)
        {
            _jwtConfig = configuration.GetSection("JwtConfig");
        }
        public string GenerateToken()
        {
            var issuer =_jwtConfig["Authority"];
            var audience = _jwtConfig["Audience"];
            var key = Encoding.ASCII.GetBytes(_jwtConfig["Secret"]);
            var signInCredentials = new SymmetricSecurityKey(key);
            var alg = SecurityAlgorithms.HmacSha512Signature;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, Constants.AdminRole),
                    new Claim(JwtRegisteredClaimNames.Sub, "raime"),
                    new Claim(JwtRegisteredClaimNames.Email, "raime@email.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signInCredentials, alg)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return $"Bearer {jwtToken}";
        }
    }
}
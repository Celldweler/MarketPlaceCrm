using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MarketPlaceCrm.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MarketPlaceCrm.WebApi.Services
{
    public class JwtManager
    {
        private readonly AppDbContext _ctx;
        private readonly IConfigurationSection _jwtConfig;

        public JwtManager(IConfiguration configuration, AppDbContext ctx)
        {
            _ctx = ctx;
            _jwtConfig = configuration.GetSection("JwtConfig");
        }
        public string GenerateToken(string userName)
        {
            var issuer =_jwtConfig["Authority"];
            var audience = _jwtConfig["Audience"];
            var key = Encoding.ASCII.GetBytes(_jwtConfig["Secret"]);
            var signInCredentials = new SymmetricSecurityKey(key);
            var alg = SecurityAlgorithms.HmacSha512Signature;
            
            var userDetail = _ctx.Customers.FirstOrDefault(x => x.Email == userName);
            if (userDetail == null)
                return "user not found";
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", userDetail.Id.ToString()),
                    new Claim(ClaimTypes.Role, userDetail.Role ?? "none"),
                    new Claim(JwtRegisteredClaimNames.Sub, userDetail.Email),
                    new Claim(JwtRegisteredClaimNames.Email, userDetail.Email),
                    // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
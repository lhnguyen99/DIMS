﻿using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DIMSApis.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User u)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,u.Email),
                new Claim(ClaimTypes.Role,u.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier,u.UserId.ToString())
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDes);
            return tokenHandler.WriteToken(token);
        }
    }
}

using Application.Contrast.Repository;
using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security.Idetity.Token
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;
        public GenerateToken(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            this.config = config;
            this.userManager = userManager;

        }
        public async Task<string> GenerateAcsessToken(int userId, string userName, string firstName, string lastName)
        {
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Name,userName),
            new Claim("lastName",lastName),
            new Claim("firstName",firstName),
            };
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("user not found");

            }
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["[jwt:SecretKey]"]
                ?? throw new InvalidOperationException("Jwt secret key is missing")));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            double expierAt = Convert.ToDouble(config["jwt:DurationInMinutes"]);
            var token = new JwtSecurityToken(
           claims: claims,
           issuer: config["jwt:Issuer"],
           audience: config["jwt:Audience"],
           expires: DateTime.UtcNow.AddMinutes(expierAt),
           signingCredentials: credential
           );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

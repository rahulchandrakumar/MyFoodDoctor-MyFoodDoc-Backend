using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MyFoodDoc.CMS.Auth.Implementation
{
    public class DebugAuthenticationService : ICustomAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public DebugAuthenticationService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public AppUser Login(string username, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AuthSecret"]);
            
            AppUser user = null;

            if (username == "admin")
            {
                user = new AppUser
                {
                    Displayname = "Admin Admin",
                    Username = "admin",
                    Roles = new string[] { "Admin", "Editor", "Approver" }
                };
            }
            else if (username == "editor")
            {
                user = new AppUser
                {
                    Displayname = "editor",
                    Username = "editor",
                    Roles = new string[] { "Editor" }
                };
            }
            else if (username == "editor2")
            {
                user = new AppUser
                {
                    Displayname = "editor2",
                    Username = "editor2",
                    Roles = new string[] { "Editor" }
                };
            }
            else if (username == "approver")
            {
                user = new AppUser
                {
                    Displayname = "approver",
                    Username = "approver",
                    Roles = new string[] { "Approver" }
                };
            }

            var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.GivenName, user.Displayname),
                        new Claim(ClaimTypes.Name, user.Username)
                    };

            if (user.Roles != null)
                userClaims.AddRange(user.Roles?.Select(role => new Claim(ClaimTypes.Role, role)).ToList());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            if (user == null)
                throw new Exception("Login failed.");

            return user;
        }
    }
}

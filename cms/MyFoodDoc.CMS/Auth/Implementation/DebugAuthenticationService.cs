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

        private readonly IList<AppUser> _users = new List<AppUser>
        {
            new AppUser
            {
                Displayname = "Admin Admin",
                Username = "admin",
                Roles = new string[] { "Admin", "Editor", "Viewer" }
            },
            new AppUser
            {
                Displayname = "editor",
                Username = "editor",
                Roles = new string[] { "Editor", "Viewer" }
            },
            new AppUser
            {
                Displayname = "editor2",
                Username = "editor2",
                Roles = new string[] { "Editor", "Viewer" }
            },
            new AppUser
            {
                Displayname = "Viewer",
                Username = "viewer",
                Roles = new string[] { "Viewer" }
            }
        };

        public DebugAuthenticationService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._users.Add(new AppUser()
            {
                Displayname = _configuration["SuperAdmin:Displayname"],
                Username = _configuration["SuperAdmin:Username"],
                Roles = new string[] { "Admin", "Editor", "Viewer" }
            });
        }

        public AppUser Login(string username, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AuthSecret"]);
            
            AppUser user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                throw new Exception("Login failed.");

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

            return user;
        }
    }
}

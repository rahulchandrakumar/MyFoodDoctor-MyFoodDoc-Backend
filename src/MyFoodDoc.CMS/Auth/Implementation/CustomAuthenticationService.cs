using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFoodDoc.CMS.Application.Common;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Auth.Implementation
{
    public class CustomAuthenticationService : ICustomAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHashingManager _hashingManager;

        public CustomAuthenticationService(IConfiguration configuration, IUserService userService, IHashingManager hashingManager)
        {
            this._userService = userService;
            this._configuration = configuration;
            this._hashingManager = hashingManager;
        }

        public async Task<AppUser> Login(string username, string password)
        {
            //if (_users.Count == 0)
            //    await InitUsers();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AuthSecret"]);

            var user = await _userService.GetByUsername(username);

            if (user == null && user.Username == _configuration["SuperAdmin:Username"])
            {
                user = new UserModel()
                {
                    Displayname = _configuration["SuperAdmin:Displayname"],
                    Username = _configuration["SuperAdmin:Username"],
                    PasswordHash = _hashingManager.HashToString(_configuration["SuperAdmin:Password"]),
                    Role = UserRoleEnum.Admin
                };
            }

            if (user == null || user.PasswordHash != null && !_hashingManager.Verify(password, user.PasswordHash))
                throw new Exception("Login failed.");

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Displayname),
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (user.Roles != null)
                userClaims.AddRange(user.Roles?.Select(role => new Claim(ClaimTypes.Role, role.ToString())).ToList());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AppUser()
            {
                Displayname = user.Displayname,
                Roles = user.Roles.Select(r => r.ToString()).ToList(),
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}

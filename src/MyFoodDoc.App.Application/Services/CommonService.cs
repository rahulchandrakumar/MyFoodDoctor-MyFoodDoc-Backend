using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System.Linq;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Exceptions;

namespace MyFoodDoc.App.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CommonService(IApplicationContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task RegisterAsync(string email, string password, int insuranceId)
        {
            User newUser = new User
            {
                UserName = email,
                Email = email,
                InsuranceId = insuranceId,
            };

            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new BadRequestException("User is not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task ResetPasswordAsync(string email, string resetToken, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) throw new BadRequestException("User is not found");
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join('\n', result.Errors.Select(x => x.Description)));
            }
        }
    }
}

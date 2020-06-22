using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task RegisterAsync(RegisterPayload payload, CancellationToken cancellationToken = default)
        {
            User newUser = new User
            {
                UserName = payload.Email,
                Email = payload.Email,
                InsuranceId = payload.InsuranceId,
            };

            var result = await _userManager.CreateAsync(newUser, payload.Password);
        }
    }
}

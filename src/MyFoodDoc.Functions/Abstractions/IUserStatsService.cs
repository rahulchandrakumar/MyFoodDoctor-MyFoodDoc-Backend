﻿using MyFoodDoc.Application.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Abstractions
{
    public interface IUserStatsService
    {
        Task<UserStatsDto> GetUserStatsAsync(CancellationToken cancellationToken = default);
    }
}
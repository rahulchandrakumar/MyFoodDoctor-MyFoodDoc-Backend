using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Clients.FatSecret;

namespace MyFoodDoc.App.Application.Clients
{
    public interface IFatSecretClient
    {
        Task<Food> GetFoodAsync(long id);
    }
}







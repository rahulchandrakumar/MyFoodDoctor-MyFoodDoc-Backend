using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyFoodDoc.FatSecretClient.Clients;


namespace MyFoodDoc.FatSecretClient.Abstractions
{
    public interface IFatSecretClient
    {
        Task<Food> GetFoodAsync(long id);
    }
}







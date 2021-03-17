using MyFoodDoc.AokClient.Clients;
using System;
using System.Threading.Tasks;

namespace MyFoodDoc.AokClient.Abstractions
{
    public interface IAokClient
    {
        Task<ValidateResponse> ValidateAsync(string insuranceNumber, DateTime birthday, string source = "myfooddock");
        Task<RegisterResponse> Register(RegisterRequest request);
        Task Delete(string token);
    }
}

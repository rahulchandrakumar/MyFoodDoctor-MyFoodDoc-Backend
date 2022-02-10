using MyFoodDoc.FatSecretClient.Clients;
using System.Threading.Tasks;


namespace MyFoodDoc.FatSecretClient.Abstractions
{
    public interface IFatSecretClient
    {
        Task<Food> GetFoodAsync(long id);
    }
}







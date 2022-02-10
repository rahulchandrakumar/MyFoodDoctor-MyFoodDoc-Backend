using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.CMS.Models;
using MyFoodDoc.CMS.Payloads;
using MyFoodDoc.FatSecretClient.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Controllers
{
    [Authorize(Roles = "Viewer")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServingsController : ControllerBase
    {
        private readonly IFatSecretClient _fatSecretClient;

        public ServingsController(IFatSecretClient fatSecretClient)
        {
            _fatSecretClient = fatSecretClient;
        }

        // GET: api/v1/Servings
        [HttpGet]
        public async Task<Serving> Get([FromQuery] ServingGetPayload payload, CancellationToken cancellationToken = default)
        {
            var food = await _fatSecretClient.GetFoodAsync(payload.FoodId);

            var serving = food.Servings.Serving.Single(x => x.Id == payload.ServingId);

            return new Serving
            {
                FoodId = food.Id,
                FoodName = food.Name,
                BrandName = food.BrandName,
                ServingId = serving.Id,
                ServingDescription = serving.Description,
                MetricServingAmount = serving.MetricServingAmount,
                MetricServingUnit = serving.MetricServingUnit,
                MeasurementDescription = serving.MeasurementDescription,
                Calories = serving.Calories,
                Carbohydrate = serving.Carbohydrate,
                Protein = serving.Protein,
                Fat = serving.Fat,
                SaturatedFat = serving.SaturatedFat,
                PolyunsaturatedFat = serving.PolyunsaturatedFat,
                MonounsaturatedFat = serving.MonounsaturatedFat,
                Cholesterol = serving.Cholesterol,
                Sodium = serving.Sodium,
                Potassium = serving.Potassium,
                Fiber = serving.Fiber,
                Sugar = serving.Sugar
            };
        }
    }
}

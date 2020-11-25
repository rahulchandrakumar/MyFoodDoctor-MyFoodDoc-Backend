using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.FatSecretClient.Abstractions;


namespace MyFoodDoc.Functions
{
    public class FatSecretSynchronization
    {
        private readonly IApplicationContext _context;
        private readonly IFatSecretClient _fatSecretClient;

        public FatSecretSynchronization(IApplicationContext context, IFatSecretClient fatSecretClient)
        {
            _context = context;
            _fatSecretClient = fatSecretClient;
        }

        [FunctionName("FatSecretSynchronization")]
        public async Task RunAsync(
            [TimerTrigger("0 */5 * * * *"/*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer, 
            ILogger log, 
            CancellationToken cancellationToken)
        {
            var ingredients = await _context.Ingredients.Where(x => x.LastSynchronized < DateTime.Now.AddDays(-1)).ToListAsync(cancellationToken);

            log.LogInformation($"{ingredients.Count()} records to update.");

            int updated = 0;

            if (ingredients.Any())
            {
                //TODO: optimize, group by FoodId
                foreach (var ingredient in ingredients)
                {
                    var food = await _fatSecretClient.GetFoodAsync(ingredient.FoodId);

                    if (food == null)
                    {
                        log.LogError($"Food with id {ingredient.FoodId} not found.");
                        continue;
                    }

                    var serving = food.Servings.Serving.SingleOrDefault(s => s.Id == ingredient.ServingId);

                    if (serving == null)
                    {
                        log.LogError($"Serving with id {ingredient.ServingId} not found.");
                        continue;
                    }

                    ingredient.FoodName = food.Name;
                    ingredient.ServingDescription = serving.Description;
                    ingredient.MetricServingAmount = serving.MetricServingAmount;
                    ingredient.MetricServingUnit = serving.MetricServingUnit;
                    ingredient.MeasurementDescription = serving.MeasurementDescription;
                    ingredient.CaloriesExternal = serving.Calories;
                    ingredient.CarbohydrateExternal = serving.Carbohydrate;
                    ingredient.ProteinExternal = serving.Protein;
                    ingredient.FatExternal = serving.Fat;
                    ingredient.SaturatedFatExternal = serving.SaturatedFat;
                    ingredient.PolyunsaturatedFatExternal = serving.PolyunsaturatedFat;
                    ingredient.MonounsaturatedFatExternal = serving.MonounsaturatedFat;
                    ingredient.CholesterolExternal = serving.Cholesterol;
                    ingredient.SodiumExternal = serving.Sodium;
                    ingredient.PotassiumExternal = serving.Potassium;
                    ingredient.FiberExternal = serving.Fiber;
                    ingredient.SugarExternal = serving.Sugar;
                    ingredient.LastSynchronized = DateTime.Now;

                    _context.Ingredients.Update(ingredient);

                    updated++;
                }

                await _context.SaveChangesAsync(cancellationToken);

                log.LogInformation($"{updated} records updated.");
            }
        }
    }
}

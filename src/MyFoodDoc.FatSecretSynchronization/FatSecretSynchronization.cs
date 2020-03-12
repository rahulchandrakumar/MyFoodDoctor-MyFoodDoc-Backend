using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.FatSecretClient.Abstractions;


namespace MyFoodDoc.FatSecretSynchronization
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

        [FunctionName("Synchronize")]
        public async Task Run([TimerTrigger("%TimerInterval%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            var ingredients = _context.Ingredients.Where(x => x.LastSynchronized < DateTime.Now.AddDays(-1)).ToList();

            log.LogInformation($"{ingredients.Count()} records to update.");

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
                    ingredient.LastSynchronized = DateTime.Now;

                    _context.Ingredients.Update(ingredient);
                }

                await _context.SaveChangesAsync(cancellationToken);

                log.LogInformation($"{ingredients.Count()} records updated.");
            }
        }
    }
}

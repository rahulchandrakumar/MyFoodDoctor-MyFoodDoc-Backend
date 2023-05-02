using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.FatSecretClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace MyFoodDoc.Functions.Functions
{
    public class FatSecretSynchronization
    {
        private readonly IApplicationContext _context;
        private readonly IFatSecretClient _fatSecretClient;
        private const int BatchSize = 300;

        public FatSecretSynchronization(
            IApplicationContext context,
            IFatSecretClient fatSecretClient)
        {
            _context = context;
            _fatSecretClient = fatSecretClient;
        }

        [FunctionName(nameof(FatSecretSynchronization))]
        public async Task RunAsync([TimerTrigger("0 0 1-4 * * *"/*"%TimerInterval%"*/, RunOnStartup = false)] TimerInfo myTimer,
            ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            log.LogInformation($"FatSecretSynchronization executed at: {DateTime.Now}");

            var ingredients = await _context.Ingredients.Where(x => x.LastSynchronized < DateTime.Now.AddHours(-22)).OrderBy(x => x.LastSynchronized).Take(BatchSize).ToListAsync();

            log.LogInformation($"{ingredients.Count} ingredients to update.");

            int currentBatchCount = 0;
            int batchSize = 40;

            var ingredientsToUpdate = new List<Ingredient>();

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

                    ingredientsToUpdate.Add(ingredient);
                    currentBatchCount++;

                    if (currentBatchCount == batchSize)
                    {
                        _context.Ingredients.UpdateRange(ingredientsToUpdate);

                        await _context.SaveChangesAsync(CancellationToken.None);

                        log.LogInformation($"{currentBatchCount} ingredients updated.");

                        currentBatchCount = 0;
                        ingredientsToUpdate = new List<Ingredient>();
                    }
                }

                if (currentBatchCount > 0)
                {
                    _context.Ingredients.UpdateRange(ingredientsToUpdate);

                    await _context.SaveChangesAsync(CancellationToken.None);

                    log.LogInformation($"{currentBatchCount} ingredients updated.");
                }
            }
        }
    }
}

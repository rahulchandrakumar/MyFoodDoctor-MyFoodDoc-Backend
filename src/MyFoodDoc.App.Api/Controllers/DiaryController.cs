using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFoodDoc.Api.Models;
using MyFoodDoc.App.Application.Mock;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class DiaryController : BaseController
    {
        [HttpGet("{date:Date}")]
        [ProducesResponseType(typeof(DiaryEntryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDate([FromRoute] string date)
        {
            DiaryEntryDto entry;
            if (date == "2019-10-21")
            {
                entry = new DiaryEntryDto
                {
                    Meals = new[]
                    {
                        new DiaryEntryDtoMeal
                        {
                            Id = 1,
                            Time = "07:25",
                            Type = MealType.Breakfast,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[0],
                                    Amount = 2
                                }
                                
                            },
                            Mood = 3,
                        }
                    },
                    Exercise = new DiaryEntryDtoExercise
                    {
                        Duration = 45,
                        LastAdded = "11:25",
                    },
                    Liquid = new DiaryEntryDtoLiquid
                    {
                        Amount = 250,
                        LastAdded = "16:08",
                    },
                };
            }
            else if (date == "2019-10-22")
            {
                entry = new DiaryEntryDto
                {
                    Meals = new[]
                    {
                        new DiaryEntryDtoMeal
                        {
                            Id = 2,
                            Time = "11:37",
                            Type = MealType.Snack,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[1],
                                    Amount = 2
                                },
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[2],
                                    Amount = 2
                                }
                            },
                            Mood = 4,
                        },
                        new DiaryEntryDtoMeal
                        {
                            Id = 2,
                            Time = "23:04",
                            Type = MealType.Snack,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[3],
                                    Amount = 2
                                }
                            },
                            Mood = 5,
                        }
                    },
                    Exercise = new DiaryEntryDtoExercise
                    {
                        Duration = 90,
                        LastAdded = "15:45",
                    },
                    Liquid = new DiaryEntryDtoLiquid
                    {
                        Amount = 1500,
                        LastAdded = "23:04",
                    },
                };
            }
            else
            {
                entry = new DiaryEntryDto
                {
                    Meals = new []
                    {
                        new DiaryEntryDtoMeal
                        {
                            Id = 2,
                            Time = "06:00",
                            Type = MealType.Breakfast,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[8],
                                    Amount = 10
                                }
                            },
                            Mood = 3,
                        },
                        new DiaryEntryDtoMeal
                        {
                            Id = 2,
                            Time = "12:00",
                            Type = MealType.Lunch,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[10],
                                    Amount = 3
                                }
                            },
                            Mood = 3,
                        },
                        new DiaryEntryDtoMeal
                        {
                            Id = 2,
                            Time = "18:00",
                            Type = MealType.Dinner,
                            Ingredients = new []
                            {
                                new DiaryEntryDtoMealIngredient
                                {
                                    Ingredient = IngredientsMock.Entries[11],
                                    Amount = 2
                                }
                            },
                            Mood = 3,
                        }
                    },
                    Exercise = new DiaryEntryDtoExercise
                    {
                        Duration = 0,
                    },
                    Liquid = new DiaryEntryDtoLiquid
                    {
                        Amount = 0,
                    },
                };
            }

            return Ok(entry);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DiaryEntryDto), StatusCodes.Status200OK)]
        [Route("{date}/meal")]
        public async Task<IActionResult> AddMeal([FromBody] MealPayload payload)
        {
            var newMeal = new DiaryEntryDtoMeal
            {
                Id = 999,
                Time = payload.Time,
                Type = payload.Type,
                Ingredients = payload.Ingredients.Select(x =>
                {
                    return new DiaryEntryDtoMealIngredient
                    {
                        Ingredient = IngredientsMock.Entries.FirstOrDefault(y => y.Id == x.IngredientId),
                        Amount = x.Amount
                    };
                }).ToList(),
                Mood = payload.Mood,
            };

            return Ok(newMeal);
        }

        [HttpPut("{date}/meal/{id}")]
        [ProducesResponseType(typeof(DiaryEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMeal([FromRoute] int id, [FromBody] MealPayload payload)
        {
            var newMeal = new DiaryEntryDtoMeal
            {
                Id = id,
                Time = payload.Time,
                Type = payload.Type,
                Ingredients = payload.Ingredients.Select(x =>
                {
                    return new DiaryEntryDtoMealIngredient
                    {
                        Ingredient = IngredientsMock.Entries.FirstOrDefault(y => y.Id == x.IngredientId),
                        Amount = x.Amount
                    };
                }).ToList(),
                Mood = payload.Mood,
            };

            return Ok(newMeal);
        }

        [HttpDelete("{date}/meal/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMeal([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPut("{date}/exercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExercise([FromBody] ExercisePayload payload)
        {
            var newExercise = new DiaryEntryDtoExercise
            {
                Duration = payload.Duration,
                LastAdded = DateTime.UtcNow.ToString("HH:mm"),
            };

            return Ok(newExercise);
        }

        [HttpPut("{date}/liquid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLiquid([FromBody] LiquidPayload payload)
        {
            var newLiquid = new DiaryEntryDtoLiquid
            {
                Amount = payload.Amount,
                LastAdded = DateTime.UtcNow.ToString("HH:mm"),
            };

            return Ok(newLiquid);
        }
    }
}
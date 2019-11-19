using MyFoodDoc.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Mock
{
    public static class IngredientsMock
    {
        public static IList<IngredientDto> Entries = new[]
        {
            new IngredientDto
            {
                Id = 0,
                Name = "Banane"
            },
            new IngredientDto
            {
                Id = 1,
                Name = "Thunfisch"
            },
            new IngredientDto
            {
                Id = 2,
                Name = "Schokolade"
            },
            new IngredientDto
            {
                Id = 3,
                Name = "Butter"
            },
            new IngredientDto
            {
                Id = 4,
                Name = "Kaffee"
            },
            new IngredientDto
            {
                Id = 5,
                Name = "Käse"
            },
            new IngredientDto
            {
                Id = 6,
                Name = "Milch"
            },
            new IngredientDto
            {
                Id = 7,
                Name = "Paprika"
            },
            new IngredientDto
            {
                Id = 8,
                Name = "Zwiebel"
            },
            new IngredientDto
            {
                Id = 9,
                Name = "Spinat"
            },
            new IngredientDto
            {
                Id = 10,
                Name = "Ei"
            },
            new IngredientDto
            {
                Id = 11,
                Name = "Vollkornbrot"
            },
            new IngredientDto
            {
                Id = 12,
                Name = "Rind"
            },
            new IngredientDto
            {
                Id = 13,
                Name = "Schwein"
            },
            new IngredientDto
            {
                Id = 14,
                Name = "Geflügel"
            }
        };
    }
}

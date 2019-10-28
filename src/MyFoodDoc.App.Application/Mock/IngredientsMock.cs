using MyFoodDoc.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Mock
{
    public static class IngredientsMock
    {
        public static IList<Ingredient> Entries = new[]
        {
            new Ingredient
            {
                Id = 0,
                Name = "Banane"
            },
            new Ingredient
            {
                Id = 1,
                Name = "Thunfisch"
            },
            new Ingredient
            {
                Id = 2,
                Name = "Schokolade"
            },
            new Ingredient
            {
                Id = 3,
                Name = "Butter"
            },
            new Ingredient
            {
                Id = 4,
                Name = "Kaffee"
            },
            new Ingredient
            {
                Id = 5,
                Name = "Käse"
            },
            new Ingredient
            {
                Id = 6,
                Name = "Milch"
            },
            new Ingredient
            {
                Id = 7,
                Name = "Paprika"
            },
            new Ingredient
            {
                Id = 8,
                Name = "Zwiebel"
            },
            new Ingredient
            {
                Id = 9,
                Name = "Spinat"
            },
            new Ingredient
            {
                Id = 10,
                Name = "Ei"
            },
            new Ingredient
            {
                Id = 11,
                Name = "Vollkornbrot"
            },
            new Ingredient
            {
                Id = 12,
                Name = "Rind"
            },
            new Ingredient
            {
                Id = 13,
                Name = "Schwein"
            },
            new Ingredient
            {
                Id = 14,
                Name = "Geflügel"
            }
        };
    }
}

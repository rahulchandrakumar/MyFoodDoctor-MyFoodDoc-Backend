using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class MealNutritionsDto
    {
        public decimal AnimalProtein { get; set; }

        public decimal PlantProtein { get; set; }

        public decimal Protein => AnimalProtein + PlantProtein;

        public decimal Sugar { get; set; }

        public decimal Vegetables { get; set; }
    }
}

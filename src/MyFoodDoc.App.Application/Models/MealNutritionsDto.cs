namespace MyFoodDoc.App.Application.Models
{
    public class MealNutritionsDto
    {
        public decimal AnimalProtein { get; set; }

        public decimal Calories { get; set; }

        public decimal PlantProtein { get; set; }

        public decimal Protein => AnimalProtein + PlantProtein;

        public decimal Sugar { get; set; }

        public decimal Vegetables { get; set; }

        public int Meals { get; set; }
    }
}

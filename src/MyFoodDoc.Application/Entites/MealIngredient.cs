namespace MyFoodDoc.Application.Entites
{
    public class MealIngredient
    {
        public int MealId { get; set; }

        public int IngredientId { get; set; }

        public int Amount { get; set; }

        public Meal Meal { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
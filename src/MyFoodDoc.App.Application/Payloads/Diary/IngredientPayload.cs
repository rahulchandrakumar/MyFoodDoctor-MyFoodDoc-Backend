namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class IngredientPayload
    {
        public long FoodId { get; set; }

        public long ServingId { get; set; }

        public decimal Amount { get; set; }
    }
}

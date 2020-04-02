namespace MyFoodDoc.Application.Entites.Methods
{
    public class ChoiceMethodChoice
    {
        public int Id { get; set; }

        public int MethodId { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public ChoiceMethod Method { get; set; }
    }

}

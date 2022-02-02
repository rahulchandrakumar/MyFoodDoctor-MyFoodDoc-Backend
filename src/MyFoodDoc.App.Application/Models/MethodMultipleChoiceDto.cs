namespace MyFoodDoc.App.Application.Models
{
    public class MethodMultipleChoiceDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsCorrect { get; set; }

        public bool CheckedByUser { get; set; }
    }
}

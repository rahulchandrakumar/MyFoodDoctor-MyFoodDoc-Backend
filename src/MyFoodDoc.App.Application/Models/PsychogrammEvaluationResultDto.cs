namespace MyFoodDoc.App.Application.Models
{
    public class PsychogrammEvaluationResultDto
    {
        public string Status { get; set; }

        public EvaluationDto Evaluation { get; set; }

        public QuestionDto ExtraQuestion { get; set; }
    }
}

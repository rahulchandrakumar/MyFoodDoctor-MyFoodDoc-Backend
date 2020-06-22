using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.Application.Entities
{
    public class ReportChoiceMethodChoice
    {
        public int ReportId { get; set; }

        public int MethodId { get; set; }

        public int ChoiceId { get; set; }

        public ReportMethod ReportMethod { get; set; }

        public ChoiceMethodChoice Choice { get; set; }
    }
}
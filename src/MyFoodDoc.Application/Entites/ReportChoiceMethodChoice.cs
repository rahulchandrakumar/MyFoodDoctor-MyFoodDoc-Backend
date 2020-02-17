using MyFoodDoc.Application.Entites.Methods;

namespace MyFoodDoc.Application.Entites
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
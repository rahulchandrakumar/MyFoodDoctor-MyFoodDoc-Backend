namespace MyFoodDoc.Application.Entities.Html
{
    public class HtmlStructure
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public string MealTemplate { get; set; }
        public string Meals { get; set; }
        public string Content { get; set; }
        public string Total { get; set; }
        public string TableHeader { get; set; }
        public string Main { get; set; }
        public string PdfFooter { get; set; }
        public string PdfHeader { get; set; }
        public MealStructure MealStructure { get; set; }
        public Margin MarginOptions { get; set; }
    }
}

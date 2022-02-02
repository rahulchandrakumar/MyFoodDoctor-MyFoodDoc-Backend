namespace MyFoodDoc.Application.Entities
{
    public class WebPage
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsDeletable { get; set; }
    }
}

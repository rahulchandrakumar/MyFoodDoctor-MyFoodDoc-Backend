namespace MyFoodDoc.CMS.Application.Models
{
    public class WebViewModel : BaseModel<int>
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Undeletable { get; set; }
    }
}

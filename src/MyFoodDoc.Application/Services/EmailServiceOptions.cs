namespace MyFoodDoc.Application.Services
{
    public class EmailServiceOptions
    {
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string SendGridApiKey { get; set; }
    }
}

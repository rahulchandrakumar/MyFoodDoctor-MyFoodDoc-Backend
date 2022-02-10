namespace MyFoodDoc.Application.Abstractions
{
    public class Attachment
    {
        public byte[] Content { get; set; }
        public string Type { get; set; }
        public string Filename { get; set; }
    }
}

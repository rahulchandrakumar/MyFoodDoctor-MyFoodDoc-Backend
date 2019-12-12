namespace MyFoodDoc.CMS.Payloads.Base
{
    public class BasePaginatedPayload<T>
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Search { get; set; }
        public T Filter { get; set; }
    }
}

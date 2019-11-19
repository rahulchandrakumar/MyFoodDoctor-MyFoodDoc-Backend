using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class HistoryModel<T> : BaseModel
    {
        public DateTimeOffset Created { get; set; }
        public T Value { get; set; }
    }
}

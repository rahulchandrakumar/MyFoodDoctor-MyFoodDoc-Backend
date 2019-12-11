using MyFoodDoc.Application.Abstractions;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class HistoryModel<T> : BaseModel<int>
    {
        public DateTimeOffset Created { get; set; }
        public T Value { get; set; }

        public static HistoryModel<T> FromEntity(AbstractTrackedValue<T> entity)
        {
            return new HistoryModel<T>()
            {
                Created = entity.Date,
                Value = entity.Value
            };
        }
    }
}

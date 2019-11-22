using MyFoodDoc.CMS.Application.Models;
using System;

namespace MyFoodDoc.CMS.Models.VMBase
{
    public class HistoryBaseModel<T>
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public T Value { get; set; }

        public static HistoryBaseModel<T> FromModel(HistoryModel<T> model)
        {
            return new HistoryBaseModel<T>()
            {
                Created = model.Created,
                Id = model.Id,
                Value = model.Value
            };
        }

        public HistoryModel<T> ToModel()
        {
            return new HistoryModel<T>()
            {
                Created = this.Created,
                Id = this.Id,
                Value = this.Value
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entities.Abstractions;
using MyFoodDoc.Application.Entities.Methods;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MethodMultipleChoiceModel : BaseModel<int>
    {
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
        public int MethodId { get; set; }

        public static MethodMultipleChoiceModel FromEntity(MethodMultipleChoice entity)
        {
            return entity == null ? null : new MethodMultipleChoiceModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                IsCorrect = entity.IsCorrect,
                MethodId = entity.MethodId
            };
        }

        public MethodMultipleChoice ToEntity()
        {
            return new MethodMultipleChoice()
            {
                Id = this.Id,
                Title = this.Title,
                IsCorrect = this.IsCorrect,
                MethodId = this.MethodId
            };
        }
    }
}

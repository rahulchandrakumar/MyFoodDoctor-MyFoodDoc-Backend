using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MethodModel : BaseModel<int>
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int TargetId { get; set; }

        public static MethodModel FromEntity(Method entity)
        {
            return entity == null ? null : new MethodModel()
            {
                Id = entity.Id,
                Type = entity.Type.ToString(),
                Title = entity.Title,
                Text = entity.Text,
                TargetId = entity.TargetId
            };
        }

        public Method ToEntity()
        {
            return new Method()
            {
                Id = this.Id,
                Type = (MethodType)Enum.Parse(typeof(MethodType), this.Type),
                Title = this.Title,
                Text = this.Text,
                TargetId = this.TargetId
            };
        }
    }
}

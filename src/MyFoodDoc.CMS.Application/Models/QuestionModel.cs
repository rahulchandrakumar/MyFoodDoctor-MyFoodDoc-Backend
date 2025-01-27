﻿using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.Application.Enums;
using System;

namespace MyFoodDoc.CMS.Application.Models
{
    public class QuestionModel : BaseModel<int>
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public bool VerticalAlignment { get; set; }

        public string Type { get; set; }

        public bool Extra { get; set; }

        public int ScaleId { get; set; }

        public static QuestionModel FromEntity(Question entity)
        {
            return entity == null ? null : new QuestionModel()
            {
                Id = entity.Id,
                Text = entity.Text,
                Order = entity.Order,
                VerticalAlignment = entity.VerticalAlignment,
                Type = entity.Type.ToString(),
                Extra = entity.Extra,
                ScaleId = entity.ScaleId
            };
        }

        public Question ToEntity()
        {
            return new Question()
            {
                Id = this.Id,
                Text = this.Text,
                Order = this.Order,
                VerticalAlignment = this.VerticalAlignment,
                Type = Enum.Parse<QuestionType>(this.Type),
                Extra = this.Extra,
                ScaleId = this.ScaleId
            };
        }
    }
}

﻿using System;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class MethodDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string OptimizationAreaKey { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public int? ParentId { get; set; }

        public int? TimeIntervalDay { get; set; }

        public int? TimeIntervalNight { get; set; }

        public bool? UserAnswerBoolean { get; set; }

        public decimal? UserAnswerDecimal { get; set; }

        public int? UserAnswerInteger { get; set; }

        public DateTime? DateAnswered { get; set; }

        public TimeSpan? TimeAnswered { get; set; }

        public ICollection<MethodMultipleChoiceDto> Choices { get; set; }

        public ICollection<MethodTextDto> Texts { get; set; }

    }
}

﻿using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Courses
{
    public class Subchapter : AbstractAuditableEntity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int ChapterId { get; set; }

        public Chapter Chapter { get; set; }
    }
}

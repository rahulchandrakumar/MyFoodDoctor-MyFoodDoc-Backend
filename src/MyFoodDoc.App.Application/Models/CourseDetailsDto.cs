﻿using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class CourseDetailsDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<ChapterDto> Chapters { get; set; }
    }
}

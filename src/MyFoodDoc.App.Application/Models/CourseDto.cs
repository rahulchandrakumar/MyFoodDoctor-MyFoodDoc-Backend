using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Models
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public int CompletedChaptersCount { get; set; }

        public int ChaptersCount { get; set; }

        public bool IsAvailable { get; set; }
    }
}

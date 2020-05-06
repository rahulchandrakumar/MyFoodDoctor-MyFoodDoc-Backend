using MyFoodDoc.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entites.Courses
{
    public class Course : AbstractAuditableEntity
    {
        public bool IsActive { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public int? ImageId { get; set; }

        public ICollection<Chapter> Chapters { get; set; }

        public Image Image { get; set; }
    }
}

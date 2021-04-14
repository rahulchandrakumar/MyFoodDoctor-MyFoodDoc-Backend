using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Courses
{
    public class CompletedCourse
    {
        public string UserId { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool NotificationSent { get; set; }

        public User User { get; set; }
    }
}

using MyFoodDoc.Application.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.CMS.Application.Models
{
    public class CourseModel : BaseModel<int>
    {
        public bool IsActive { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public ImageModel Image { get; set; }

        public int UsersCount { get; set; }

        public int CompletedByUsersCount { get; set; }

        public static CourseModel FromEntity(Course entity, int usersCount, int completedByUsersCount)
        {
            return entity == null ? null : new CourseModel()
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                Title = entity.Title,
                Text = entity.Text,
                Order = entity.Order,
                Image = ImageModel.FromEntity(entity.Image),
                UsersCount = usersCount,
                CompletedByUsersCount = completedByUsersCount
            };
        }

        public Course ToEntity()
        {
            return new Course()
            {
                Id = this.Id,
                IsActive = this.IsActive,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                ImageId = this.Image.Id,
            };
        }
    }
}

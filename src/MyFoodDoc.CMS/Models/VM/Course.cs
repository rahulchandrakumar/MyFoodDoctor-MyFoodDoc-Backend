using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Models.VMBase;

namespace MyFoodDoc.CMS.Models.VM
{
    public class Course : VMBase.BaseModel<int>
    {
        public bool IsActive { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        public Image Image { get; set; }

        public static Course FromModel(CourseModel model)
        {
            return model == null ? null : new Course()
            {
                Id = model.Id,
                IsActive = model.IsActive,
                Title = model.Title,
                Text = model.Text,
                Order = model.Order,
                Image = Image.FromModel(model.Image)
            };
        }

        public CourseModel ToModel()
        {
            return new CourseModel()
            {
                Id = this.Id,
                IsActive = this.IsActive,
                Title = this.Title,
                Text = this.Text,
                Order = this.Order,
                Image = this.Image.ToModel()
            };
        }
    }
}

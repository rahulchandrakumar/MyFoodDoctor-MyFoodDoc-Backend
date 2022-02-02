using MyFoodDoc.Application.Entities.Methods;

namespace MyFoodDoc.CMS.Application.Models
{
    public class MethodTextModel : BaseModel<int>
    {
        public int MethodId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public static MethodTextModel FromEntity(MethodText entity)
        {
            return entity == null ? null : new MethodTextModel()
            {
                Id = entity.Id,
                Code = entity.Code,
                Title = entity.Title,
                Text = entity.Text,
                MethodId = entity.MethodId
            };
        }

        public MethodText ToEntity()
        {
            return new MethodText()
            {
                Id = this.Id,
                Code = this.Code,
                Title = this.Title,
                Text = this.Text,
                MethodId = this.MethodId
            };
        }
    }
}

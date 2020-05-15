using MyFoodDoc.Application.EnumEntities;

namespace MyFoodDoc.CMS.Application.Models
{
    public class OptimizationAreaModel : BaseModel<int>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public int? ImageId { get; set; }

        public ImageModel Image { get; set; }

        public decimal? UpperLimit { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? Optimal { get; set; }

        public static OptimizationAreaModel FromEntity(OptimizationArea entity)
        {
            return entity == null ? null : new OptimizationAreaModel()
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name,
                Text = entity.Text,       
                Image = ImageModel.FromEntity(entity.Image),
                UpperLimit = entity.UpperLimit,
                LowerLimit = entity.LowerLimit,
                Optimal = entity.Optimal
            };
        }

        public OptimizationArea ToEntity()
        {
            return new OptimizationArea()
            {
                Id = this.Id,
                Key = this.Key,
                Name = this.Name,
                Text = this.Text,
                ImageId = this.Image.Id,
                UpperLimit = this.UpperLimit,
                LowerLimit = this.LowerLimit,
                Optimal = this.Optimal
            };
        }
    }
}

using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Application.Models
{
    public class IngredientDto : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public long FoodId { get; set; }

        public string FoodName { get; set; }

        public long ServingId { get; set; }

        public string ServingDescription { get; set; }

        public decimal MetricServingAmount { get; set; }

        public string MetricServingUnit { get; set; }

        public string MeasurementDescription { get; set; }

        public decimal Calories { get; set; }

        public decimal Carbohydrate { get; set; }

        public decimal Protein { get; set; }

        public decimal Fat { get; set; }

        public decimal SaturatedFat { get; set; }

        public decimal PolyunsaturatedFat { get; set; }

        public decimal MonounsaturatedFat { get; set; }

        public decimal Cholesterol { get; set; }

        public decimal Sodium { get; set; }

        public decimal Potassium { get; set; }

        public decimal Fiber { get; set; }

        public decimal Sugar { get; set; }

        public decimal Vegetables { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ingredient, IngredientDto>()
                .ForMember(x => x.Calories, opt => opt.MapFrom(src => src.Calories ?? src.CaloriesExternal))
                .ForMember(x => x.Carbohydrate, opt => opt.MapFrom(src => src.Carbohydrate ?? src.CarbohydrateExternal))
                .ForMember(x => x.Protein, opt => opt.MapFrom(src => src.Protein ?? src.ProteinExternal))
                .ForMember(x => x.Fat, opt => opt.MapFrom(src => src.Fat ?? src.FatExternal))
                .ForMember(x => x.SaturatedFat, opt => opt.MapFrom(src => src.SaturatedFat ?? src.SaturatedFatExternal))
                .ForMember(x => x.PolyunsaturatedFat, opt => opt.MapFrom(src => src.PolyunsaturatedFat ?? src.PolyunsaturatedFatExternal))
                .ForMember(x => x.MonounsaturatedFat, opt => opt.MapFrom(src => src.Calories ?? src.MonounsaturatedFatExternal))
                .ForMember(x => x.Cholesterol, opt => opt.MapFrom(src => src.Cholesterol ?? src.CholesterolExternal))
                .ForMember(x => x.Sodium, opt => opt.MapFrom(src => src.Sodium ?? src.SodiumExternal))
                .ForMember(x => x.Potassium, opt => opt.MapFrom(src => src.Potassium ?? src.PotassiumExternal))
                .ForMember(x => x.Fiber, opt => opt.MapFrom(src => src.Fiber ?? src.FiberExternal))
                .ForMember(x => x.Sugar, opt => opt.MapFrom(src => src.Sugar ?? src.SugarExternal));
        }
    }
}
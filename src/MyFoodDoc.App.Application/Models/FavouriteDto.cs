using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities.Diary;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.App.Application.Models
{
    public class FavouriteDto : IMapFrom<Favourite>
    {
        public int Id { get; set; }

        public string Title { get; set; }

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

        public ICollection<FavouriteIngredientDto> Ingredients { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Favourite, FavouriteDto>()
                .ForMember(x => x.Calories, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Calories ?? x.Ingredient.CaloriesExternal) * x.Amount)))
                .ForMember(x => x.Carbohydrate, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Carbohydrate ?? x.Ingredient.CarbohydrateExternal) * x.Amount)))
                .ForMember(x => x.Protein, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Protein ?? x.Ingredient.ProteinExternal) * x.Amount)))
                .ForMember(x => x.Fat, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Fat ?? x.Ingredient.FatExternal) * x.Amount)))
                .ForMember(x => x.SaturatedFat, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.SaturatedFat ?? x.Ingredient.SaturatedFatExternal) * x.Amount)))
                .ForMember(x => x.PolyunsaturatedFat, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.PolyunsaturatedFat ?? x.Ingredient.PolyunsaturatedFatExternal) * x.Amount)))
                .ForMember(x => x.MonounsaturatedFat, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Calories ?? x.Ingredient.MonounsaturatedFatExternal) * x.Amount)))
                .ForMember(x => x.Cholesterol, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Cholesterol ?? x.Ingredient.CholesterolExternal) * x.Amount)))
                .ForMember(x => x.Sodium, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Sodium ?? x.Ingredient.SodiumExternal) * x.Amount)))
                .ForMember(x => x.Potassium, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Potassium ?? x.Ingredient.PotassiumExternal) * x.Amount)))
                .ForMember(x => x.Fiber, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Fiber ?? x.Ingredient.FiberExternal) * x.Amount)))
                .ForMember(x => x.Sugar, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Sugar ?? x.Ingredient.SugarExternal) * x.Amount)))
                .ForMember(x => x.Vegetables, opt => opt.MapFrom(src => src.Ingredients.Sum(x => (x.Ingredient.Vegetables ?? 0) * x.Amount)));
        }
    }
}

using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IEntityMapper
    {
        DiaryEntryDtoMeal ToDto(Meal entity);
    }
}

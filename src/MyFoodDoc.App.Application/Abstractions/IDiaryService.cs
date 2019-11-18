using MyFoodDoc.Api.Models;
using System;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IDiaryService
    {
        Task<DiaryEntryDto> GetAll(DateTime start);
    }
}

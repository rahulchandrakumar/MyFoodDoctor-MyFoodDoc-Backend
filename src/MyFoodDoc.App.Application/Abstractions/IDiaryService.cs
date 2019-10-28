using MyFoodDoc.Api.Models;
using System;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IDiaryService
    {
        Task<DiaryEntry> GetAll(DateTime start);
    }
}

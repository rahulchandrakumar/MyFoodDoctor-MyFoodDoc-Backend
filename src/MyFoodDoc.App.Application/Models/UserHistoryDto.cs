using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities;
using System.Linq;

namespace MyFoodDoc.App.Application.Models
{
    public class UserHistoryDto
    {
        public UserHistoryDtoWeight Weight { get; set; }

        public UserHistoryDtoAbdominalGirth AbdominalGirth { get; set; }
    }
}
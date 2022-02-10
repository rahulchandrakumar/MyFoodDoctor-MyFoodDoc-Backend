using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Course;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ICourseService
    {
        Task<ICollection<CourseDto>> GetAsync(string userId, CancellationToken cancellationToken);

        Task<CourseDetailsDto> GetDetailsAsync(string userId, int courseId, CancellationToken cancellationToken);

        Task InsertAnswerAsync(string userId, int courseId, AnswerPayload payload, CancellationToken cancellationToken);
    }
}

﻿using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ICourseService : IServiceBasePaginatedRead<CourseModel, int>, IServiceBaseWrite<CourseModel, int>
    {
    }
}

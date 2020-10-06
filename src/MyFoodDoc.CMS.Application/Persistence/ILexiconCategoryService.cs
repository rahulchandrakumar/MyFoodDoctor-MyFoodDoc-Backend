using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ILexiconCategoryService : IServiceBasePaginatedRead<LexiconCategoryModel, int>, IServiceBaseWrite<LexiconCategoryModel, int>
    {
    }
}

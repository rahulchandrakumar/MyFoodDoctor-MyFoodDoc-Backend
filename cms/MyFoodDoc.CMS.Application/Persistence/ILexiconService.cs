﻿using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence.Base;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface ILexiconService : IServiceBaseRead<LexiconModel, int>, IServiceBaseWrite<LexiconModel, int>
    {
    }
}
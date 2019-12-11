using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.CMS.Application.FilterModels
{
    public enum IngredientFilterState: byte
    {
        HaveToSpecify = 0,
        NotSpecified,
        Specified
    }

    public class IngredientFilter
    {
        public IngredientFilterState State { get; set; }
    }
}

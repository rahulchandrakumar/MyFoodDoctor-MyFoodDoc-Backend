using DotNetify;
using DotNetify.Security;
using MyFoodDoc.CMS.Models.VMBase;
using MyFoodDoc.CMS.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels
{
    [Authorize("Editor")]
    public class PortionsColabViewModel : BaseStateViewModel<ColabDataTableBaseModel<int>, int>
    {
        public PortionsColabViewModel(IConnectionContext connectionContext) : base(connectionContext)
        {
        }

        protected override Func<Task<IList<ColabDataTableBaseModel<int>>>> GetData => async () => {
            return await Task.FromResult(new List<ColabDataTableBaseModel<int>>());
        };
    }
}

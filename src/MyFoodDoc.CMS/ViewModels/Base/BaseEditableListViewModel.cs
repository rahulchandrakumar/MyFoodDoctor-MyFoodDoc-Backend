using DotNetify;
using MyFoodDoc.CMS.Models.VMBase;
using System;

namespace MyFoodDoc.CMS.ViewModels.Base
{
    public abstract class BaseEditableListViewModel<T1, T2> : BaseListViewModel<T1, T2> where T1 : ColabDataTableBaseModel<T2> where T2 : IEquatable<T2>
    {
        public BaseEditableListViewModel(IConnectionContext connectionContext) : base(connectionContext)
        {
        }

        public abstract Action<T1> Add { get; }
        public abstract Action<T1> Update { get; }
        public abstract Action<T2> Remove { get; }
    }
}

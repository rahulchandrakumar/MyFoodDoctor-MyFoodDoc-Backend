using DotNetify;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.ViewModels
{
    public class BaseListViewModel<T> : MulticastVM
    {
        public string Items_itemKey => "Id";
        public IList<T> Items = new List<T>();
        public bool IsLoaded = true;
    }
}

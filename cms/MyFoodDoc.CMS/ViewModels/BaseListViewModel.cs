using DotNetify;
using MyFoodDoc.CMS.Models;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.ViewModels
{
    public class BaseListViewModel<T> : MulticastVM where T: ColabDataTableBaseModel
    {
        public string Items_itemKey => "Id";
        public IList<T> Items = new List<T>();
        public bool IsLoaded = true;
    }
}

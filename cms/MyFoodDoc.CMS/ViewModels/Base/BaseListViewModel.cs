using DotNetify;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels.Base
{
    public abstract class BaseListViewModel<T1, T2> : MulticastVM where T1: ColabDataTableBaseModel<T2> where T2: IEquatable<T2>
    {
        public string Items_itemKey => "Id";
        public IList<T1> Items = new List<T1>();
        public bool IsLoaded = false;

        private readonly object loadLock = new object();

        protected abstract Func<Task<IList<T1>>> GetData { get; }

        public Action Init => async () =>
        {
            try
            {
                if (!IsLoaded)
                {
                    var data = await GetData();
                    lock (loadLock)
                        if (!IsLoaded)
                        {
                            IsLoaded = true;
                            this.Set(true, nameof(IsLoaded));

                            this.SetList(data);
                        }
                }
            }
            catch (Exception ex)
            {

            }
        };

        public Action<T1> BeginEdit => (T1 item) =>
        {
            try
            {
                var edited = this.Items.First(x => EqualityComparer<T2>.Default.Equals(x.Id, item.Id));
                edited.Editor = item.Editor;
                edited.LockDate = item.LockDate;

                this.UpdateList(nameof(Items), edited);
                this.PushUpdates();
            }
            catch (Exception ex)
            {

            }
        };

        public Action<T2> CancelEdit => (T2 id) =>
        {
            try
            {
                var edited = this.Items.First(x => EqualityComparer<T2>.Default.Equals(x.Id, id));
                edited.Editor = null;
                edited.LockDate = null;

                this.UpdateList(nameof(Items), edited);
                this.PushUpdates();
            }
            catch (Exception ex)
            {

            }
        };

        public void SetList(IList<T1> list)
        {
            this.Items = list;
            this.Set(list, nameof(Items));

            this.PushUpdates();
        }

        public void AddList(T1 item)
        {
            this.Items.Add(item);
            this.AddList(nameof(Items), item);

            this.PushUpdates();
        }

        public void UpdateList(T1 item)
        {
            this.Items.Remove(this.Items.First(x => EqualityComparer<T2>.Default.Equals(x.Id, item.Id)));
            this.Items.Add(item);

            this.UpdateList(nameof(Items), item);
            this.PushUpdates();
        }

        public void RemoveList(T2 Id)
        {
            this.Items.Remove(this.Items.First(x => EqualityComparer<T2>.Default.Equals(x.Id, Id)));

            this.RemoveList(nameof(Items), Id);
            this.PushUpdates();
        }
    }
}

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
        public bool HasError = false;
        public string ErrorMessage = null;

        private readonly object loadLock = new object();

        protected abstract Func<Task<IList<T1>>> GetData { get; }

        private readonly IConnectionContext _connectionContext;
        private readonly List<string> _connectionIds = new List<string>();

        public BaseListViewModel(IConnectionContext connectionContext)
        {
            this._connectionContext = connectionContext;
        }

        public Action Init => async () =>
        {
            try
            {
                this._connectionIds.Add(_connectionContext.ConnectionId);

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
                SendError(ex);
            }
        };

        public Action<T1> BeginEdit => (T1 item) =>
        {
            try
            {
                var edited = this.Items.FirstOrDefault(x => EqualityComparer<T2>.Default.Equals(x.Id, item.Id));
                if (edited == null)
                {
                    edited = (T1)Activator.CreateInstance(typeof(T1));
                    this.Items.Add(edited);
                    this.AddList(nameof(Items), edited);
                }

                edited.Id = item.Id;
                edited.Editor = item.Editor;
                edited.LockDate = item.LockDate;

                this.UpdateList(nameof(Items), edited);
                this.PushUpdates();
            }
            catch (Exception ex)
            {
                SendError(ex);
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
                SendError(ex);
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

        internal void SendError(Exception ex)
        {
            var message = ex.Message;

            base.Send(new List<string>() { _connectionContext.ConnectionId }, "Error", new { ErrorMessage = message });
        }
    }
}

using DotNetify;
using MyFoodDoc.CMS.Models.VMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.ViewModels.Base
{
    public abstract class BaseStateViewModel<T1, T2> : MulticastVM where T1: ColabDataTableBaseModel<T2> where T2: IEquatable<T2>
    {
        public string Items_itemKey => "Id";
        public IList<T1> State = new List<T1>();
        public bool IsLoaded = false;

        private readonly object loadLock = new object();

        protected abstract Func<Task<IList<T1>>> GetData { get; }

        internal readonly IConnectionContext _connectionContext;
        internal readonly List<string> _connectionIds = new List<string>();

        public BaseStateViewModel(IConnectionContext connectionContext)
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
                Error(ex);
            }
        };

        public Action<T1> BeginEdit => (T1 item) =>
        {
            try
            {
                var edited = this.State.FirstOrDefault(x => EqualityComparer<T2>.Default.Equals(x.Id, item.Id));
                if (edited == null)
                {
                    edited = (T1)Activator.CreateInstance(typeof(T1));
                    this.State.Add(edited);
                    this.AddList(nameof(State), edited);
                }

                edited.Id = item.Id;
                edited.Editor = item.Editor;
                edited.LockDate = item.LockDate;

                this.UpdateList(nameof(State), edited);
                this.PushUpdates();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        };

        public Action<T2> CancelEdit => (T2 id) =>
        {
            try
            {
                var edited = this.State.First(x => EqualityComparer<T2>.Default.Equals(x.Id, id));
                edited.Editor = null;
                edited.LockDate = null;

                this.UpdateList(nameof(State), edited);
                this.PushUpdates();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        };

        public void SetList(IList<T1> list)
        {
            this.State = list;
            this.Set(list, nameof(State));

            this.PushUpdates();
        }

        public void AddList(T1 item)
        {
            this.State.Add(item);
            this.AddList(nameof(State), item);

            this.PushUpdates();
        }

        public void UpdateList(T1 item)
        {
            this.State.Remove(this.State.First(x => EqualityComparer<T2>.Default.Equals(x.Id, item.Id)));
            this.State.Add(item);

            this.UpdateList(nameof(State), item);
            this.PushUpdates();
        }

        public void RemoveList(T2 Id)
        {
            this.State.Remove(this.State.First(x => EqualityComparer<T2>.Default.Equals(x.Id, Id)));

            this.RemoveList(nameof(State), Id);
            this.PushUpdates();
        }

        public Action<ColabDataTableBaseModel<int>> Add => (item) =>
        {
            base.Send(_connectionIds, "Added", new { Id = item.Id });
        };

        public Action<ColabDataTableBaseModel<int>> Update => (item) =>
        {
            base.Send(_connectionIds, "Updated", new { Id = item.Id });
        };

        public Action<int> Remove => (id) =>
        {
            base.Send(_connectionIds, "Deleted", new { Id = id });
        };

        internal void Error(Exception ex)
        {
            var message = ex.Message;

            base.Send(new List<string>() { _connectionContext.ConnectionId }, "Error", new { ErrorMessage = message });
        }
    }
}

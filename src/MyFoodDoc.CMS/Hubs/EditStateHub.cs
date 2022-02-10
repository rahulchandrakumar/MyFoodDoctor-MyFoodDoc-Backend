using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyFoodDoc.CMS.Models.VMBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Hubs
{
    [Authorize]
    public class EditStateHub : Hub
    {
        private static readonly Dictionary<string, Dictionary<int, EditStateHubModel>> _groupedEditList = new Dictionary<string, Dictionary<int, EditStateHubModel>>();

        public const string ItemAddedMsg = "ItemAdded";
        public const string ItemUpdatedMsg = "ItemUpdated";
        public const string ItemDeletedMsg = "ItemDeleted";

        public async Task Init(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("StateList",
                groupName,
                (_groupedEditList.ContainsKey(groupName) ? _groupedEditList[groupName].Values as IEnumerable<EditStateHubModel> : new List<EditStateHubModel>()).ToDictionary(k => k.Id, v => v));
        }

        public async Task Close(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task AddEntry(string groupName, EditStateHubModel model)
        {
            if (!_groupedEditList.ContainsKey(groupName))
                _groupedEditList[groupName] = new Dictionary<int, EditStateHubModel>();

            _groupedEditList[groupName][model.Id] = model;
            await Clients.Group(groupName).SendAsync("AddedState", groupName, model);
        }

        public async Task RemoveEntry(string groupName, int id)
        {
            _groupedEditList[groupName].Remove(id);

            if (_groupedEditList[groupName].Count == 0)
                _groupedEditList.Remove(groupName);

            await Clients.Group(groupName).SendAsync("RemovedState", groupName, id);
        }
    }
}

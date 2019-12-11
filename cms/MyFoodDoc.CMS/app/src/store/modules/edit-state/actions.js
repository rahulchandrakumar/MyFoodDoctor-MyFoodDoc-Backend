import { HubConnectionBuilder } from "@microsoft/signalr"
import { MessagePackHubProtocol } from "@microsoft/signalr-protocol-msgpack"

export default {
  init: async ({ commit, state }, { groupName }) => {
    var connection = state.connection;
    if (connection == null) {
      connection = new HubConnectionBuilder()
                      .withUrl(process.env.VUE_APP_WEB_API_URL + "/edit-states")
                      .withHubProtocol(new MessagePackHubProtocol())
                      .withAutomaticReconnect()
                      .build();

      connection.on("StateList", (groupName, states) => commit("setStates", { stateItems: states, groupName }));
      connection.on("AddedState", (groupName, state) => commit("addState", { stateItem: state, groupName }));
      connection.on("RemovedState", (groupName, id) => commit("removeState", { id: id, groupName }));

      connection.on("ItemAdded", (groupName, id) => commit("itemAdded", { id: id, groupName }));
      connection.on("ItemUpdated", (groupName, id) => commit("itemUpdated", { id: id, groupName }));
      connection.on("ItemDeleted", (groupName, id) => commit("itemDeleted", { id: id, groupName }));

      await connection.start()
      commit("setConnection", connection)
    }

    if (state.StateItems[groupName] == null) {
      await connection.invoke('Init', groupName).catch(function (err) {
        return console.error(err.toString());
      })
    }
  },
  removeGroup: async ({ commit, state }, { groupName }) => {
    await state.connection.invoke("Close", groupName);
    commit('removeGroup', { groupName })
  },
  addEntry: async ({ state }, { groupName, entry }) => {
    await state.connection.invoke("AddEntry", groupName, entry);
  },
  removeEntry: async ({ state }, { groupName, id }) => {
    await state.connection.invoke("RemoveEntry", groupName, id);
  },
}
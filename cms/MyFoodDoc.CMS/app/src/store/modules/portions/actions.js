import integration from "@/integration";

export default {
  loadItems: async ({ commit, state }) => {
    state.loaded = false

    let response = await integration.portions.getAll();
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    commit("setItems", response.data);

    return state.items;
  },
  loadItem: async ({ state }, { id }) => {
    state.loaded = false

    let response = await integration.portions.get(id);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return response.data;
  },
  addItem: async ({ state }, { item }) => {
    state.loaded = false

    let response = await integration.portions.post(item);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return state.items;
  },
  updateItem: async ({ state }, { item }) => {
    state.loaded = false

    let response = await integration.portions.put(item);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return state.items;
  },
  deleteItem: async ({ state }, { item }) => {
    state.loaded = false

    let response = await integration.portions.put(item);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return state.items;
  },
  itemAdded: async ({ commit, dispatch }, { Id }) => {
    var item = await dispatch('loadItem', { id: Id })
    commit("addItem", item)
  },
  itemUpdated: async ({ commit, dispatch }, { Id }) => {
    var item = await dispatch('loadItem', { id: Id })
    commit("setItem", item)
  },
  itemDeleted: async ({ commit }, { Id }) => {
    commit("deleteItem", Id)
  },
};
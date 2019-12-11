import integration from "@/integration";

export default {
  loadItems: async ({ commit, state }, { page, search, filter }) => {
    state.loaded = false

    state.skip = (page - 1) * state.take;
    state.search = search;
    state.filter = filter;

    let response = await integration.portions.getAll({ take: state.take, skip: state.skip, search: state.search, filter: state.filter });
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
  loadOneMoreItem: async ({ state }) => {
    state.loaded = false

    let response = await integration.portions.getAll({ take: 1, skip: state.skip + state.take - 1, search: state.search });
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return response.data.values.length > 0 ? response.data.values[0] : null;
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
  itemAdded: async ({ state, commit, dispatch }, { Id }) => {
    if (state.skip == 0) {
      var item = await dispatch('loadItem', { id: Id })
      commit("addItem", item)
    }
    state.total++
  },
  itemUpdated: async ({ state, commit, dispatch }, { Id }) => {
    if (state.items.filter(i => i.id == Id).length > 0) {
      var item = await dispatch('loadItem', { id: Id })
      commit("setItem", item)
    }
  },
  itemDeleted: async ({ state, commit }, { Id }) => {
    if (state.items.filter(i => i.id == Id).length > 0) {
      var item = await dispatch('loadOneMoreItem')
      commit("deleteItem", { Id, item })
    }
    state.total--
  },
};
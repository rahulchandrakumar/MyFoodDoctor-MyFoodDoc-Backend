import integration from "@/integration";

export default {
  loadItems: async ({ commit, state, dispatch }, { page, search }) => {
    state.loaded = false

    state.skip = (page - 1) * state.take;
    state.search = search;

    let response = await integration.promotions.getAll({ take: state.take, skip: state.skip, search: state.search });
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    await dispatch("mutateData", { data: response.data })
    commit("setItems", response.data);

    return state.items;
  },
  loadItem: async ({ state, dispatch }, { id }) => {
    state.loaded = false

    let response = await integration.promotions.get(id);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    await dispatch("mutateItem", { data: response.data })

    return response.data;
  },
  loadOneMoreItem: async ({ state }) => {
    state.loaded = false

    let response = await integration.promotions.getAll({ take: 1, skip: state.skip + state.take - 1, search: state.search });
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    await dispatch("mutateData", { data: response.data })

    return response.data.values.length > 0 ? response.data.values[0] : null;
  },
  addItem: async ({ state }, { item }) => {
    state.loaded = false

    let response = await integration.promotions.post(item);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return state.items;
  },
  updateItem: async ({ state }, { item }) => {
    state.loaded = false

    let response = await integration.promotions.put(item);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return state.items;
  },
  deleteItem: async ({ state }, { id }) => {
    state.loaded = false

    let response = await integration.promotions.delete(id);
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
    var item = null;
    if (state.items.filter(i => i.id == Id).length > 0 && state.total > state.take) {
      item = await dispatch('loadOneMoreItem')
    }
    commit("deleteItem", { Id, item })
    state.total--
  },
  mutateData: async ({ commit, dispatch }, { data }) => {
    let insuranceList = await dispatch("dictionaries/getInsuranceList", {},  { root:true })
    data.values.forEach((i) => {
       i.insurance = i.insuranceId == null ? null : insuranceList.filter(v => v.id == i.insuranceId )[0].name
    })
  },
  mutateItem: async ({ commit, dispatch }, { data }) => {
    let insuranceList = await dispatch("dictionaries/getInsuranceList", {},  { root:true })
    data.insurance = data.insuranceId == null ? null : insuranceList.filter(v => v.id == data.insuranceId )[0].name
  }
};
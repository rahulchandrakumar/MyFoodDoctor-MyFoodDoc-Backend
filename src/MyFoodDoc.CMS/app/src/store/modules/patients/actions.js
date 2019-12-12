import integration from "@/integration";

export default {
  loadItems: async ({ commit, state }, { page, search }) => {
    state.loaded = false

    state.skip = (page - 1) * state.take;
    state.search = search;

    let response = await integration.patients.getAll({ take: state.take, skip: state.skip, search: state.search });
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    commit("setItems", response.data);

    return state.items;
  },
  loadItem: async ({ state }, { id }) => {
    state.loaded = false

    let response = await integration.patients.get(id);
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }

    return response.data;
  }
};
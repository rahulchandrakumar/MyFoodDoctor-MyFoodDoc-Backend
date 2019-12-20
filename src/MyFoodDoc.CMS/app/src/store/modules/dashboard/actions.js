import integration from "@/integration";

export default {
  loadItems: async ({ commit, state }) => {
    state.loaded = false

    let response = await integration.dashboard.get();
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    commit("setItems", response.data);
  }
};
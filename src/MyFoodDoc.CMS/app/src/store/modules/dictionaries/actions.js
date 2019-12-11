import integration from "@/integration";

export default {
  getinsuranceList: async ({ commit, state }) => {
    if (state.insuranceList != null)
      return state.insuranceList;

    let response = await integration.dictionaries.insurance();
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    commit("setInsuranceList", response.data);

    return state.insuranceList;
  }
};
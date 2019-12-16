import integration from "@/integration";
import Gender from "@/enums/Gender";

export default {
  loadItems: async ({ commit, state, dispatch }, { page, search }) => {
    state.loaded = false

    state.skip = (page - 1) * state.take;
    state.search = search;

    let response = await integration.patients.getAll({ take: state.take, skip: state.skip, search: state.search });
    if (response.status !== 200) {
      throw new Error(`undefined error in backend (${response.status})`);
    }
    await dispatch("mutateData", { data: response.data })
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
  },
  mutateData: async ({ commit, dispatch }, { data }) => {    
    let insuranceList = await dispatch("dictionaries/getinsuranceList", {},  { root:true })
    data.values.forEach((i) => {
       i.insurance = i.insuranceId == null ? null : insuranceList.filter(v => v.id == i.insuranceId )[0].name
       i.gender = i.gender == null ? null : i.gender == Gender.MALE ? "Male" : "Female"
    })
  }
};
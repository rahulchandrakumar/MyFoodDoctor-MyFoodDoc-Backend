import integration from "@/integration";

export default {
    getDietList: async ({ commit, state }) => {
        if (state.dietList != null)
            return state.dietList;

        let response = await integration.dictionaries.diet();
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setDietList", response.data);

        return state.dietList;
    },
    getIndicationList: async ({ commit, state }) => {
        if (state.indicationList != null)
            return state.indicationList;

        let response = await integration.dictionaries.indication();
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setIndicationList", response.data);

        return state.indicationList;
    },
    getInsuranceList: async ({ commit, state }) => {
        if (state.insuranceList != null)
            return state.insuranceList;

        let response = await integration.dictionaries.insurance();
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setInsuranceList", response.data);

        return state.insuranceList;
    },
    getMotivationList: async ({ commit, state }) => {
        if (state.motivationList != null)
            return state.motivationList;

        let response = await integration.dictionaries.motivation();
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setMotivationList", response.data);

        return state.motivationList;
    },
    getTargetList: async ({ commit, state }) => {
        if (state.targetList != null)
            return state.targetList;

        let response = await integration.dictionaries.target();
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setTargetList", response.data);

        return state.targetList;
    }
};
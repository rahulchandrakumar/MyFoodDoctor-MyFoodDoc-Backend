import integration from "@/integration";

export default {
    loadItems: async ({ commit, state }, { page, search, parentId }) => {
        state.loaded = false

        state.skip = (page - 1) * state.take;
        state.search = search;

        let response = await integration.methodmultiplechoices.getAll({ methodId: parentId, take: state.take, skip: state.skip, search: state.search });
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }
        commit("setItems", response.data);

        return state.items;
    },
    loadItem: async ({ state }, { id }) => {
        state.loaded = false

        let response = await integration.methodmultiplechoices.get(id);
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }

        return response.data;
    },
    loadOneMoreItem: async ({ state }, { parentId }) => {
        state.loaded = false

        let response = await integration.methodmultiplechoices.getAll({ methodId: parentId, take: 1, skip: state.skip + state.take - 1, search: state.search });
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }

        return response.data.values.length > 0 ? response.data.values[0] : null;
    },
    addItem: async ({ state }, { item }) => {
        state.loaded = false

        let response = await integration.methodmultiplechoices.post(item);
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }

        return state.items;
    },
    updateItem: async ({ state }, { item }) => {
        state.loaded = false

        let response = await integration.methodmultiplechoices.put(item);
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }

        return state.items;
    },
    deleteItem: async ({ state }, { id }) => {
        state.loaded = false

        let response = await integration.methodmultiplechoices.delete(id);
        if (response.status !== 200) {
            throw new Error(`undefined error in backend (${response.status})`);
        }

        return state.items;
    },
    itemAdded: async ({ state, commit, dispatch }, { Id }) => {
        var item = null;
        if (state.total - state.skip < state.take) {
            item = await dispatch('loadItem', { id: Id })
        }
        commit("addItem", item)
        state.total++
    },
    itemUpdated: async ({ state, commit, dispatch }, { Id }) => {
        var item = null;
        if (state.items.filter(i => i.id == Id).length > 0) {
            item = await dispatch('loadItem', { id: Id })
        }
        commit("setItem", item)
    },
    itemDeleted: async ({ state, commit, dispatch }, { Id, ParentId }) => {
        var item = null;
        if (state.items.filter(i => i.id == Id).length > 0 && state.total - state.skip > state.take) {
            item = await dispatch('loadOneMoreItem', { parentId: ParentId })
        }
        commit("deleteItem", { Id, item })
        state.total--
    },
};
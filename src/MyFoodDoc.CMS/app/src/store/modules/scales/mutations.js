export default {
    setItems: (state, data) => {
        state.items = data.values
        state.total = data.total
        state.loaded = true
    },
    setItem: (state, data) => {
        if (data) {
            let item = state.items.filter((i) => i.id == data.id)[0]
            Object.assign(item, data)
        }

        state.loaded = true
    },
    addItem: (state, data) => {
        if (data)
            state.items.unshift(data)

        state.loaded = true
    },
    deleteItem: (state, data) => {
        state.items = state.items.filter((i) => i.id != data.Id)
        if (data.item)
            state.items.push(data.item)

        state.loaded = true
    }
};
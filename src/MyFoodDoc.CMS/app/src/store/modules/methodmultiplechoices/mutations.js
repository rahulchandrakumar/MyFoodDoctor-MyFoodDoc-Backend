export default {
  setItems: (state, data) => {
    state.items = data.values
    state.total = data.total
    state.loaded = true
  },
  setItem: (state, data) => {
    let item = state.items.filter((i) => i.id == data.id)[0]
    Object.assign(item, data)
    state.loaded = true
  },
  addItem: (state, data) => {
    state.items.unshift(data)
    state.loaded = true
  },
  deleteItem: (state, data) => {
    state.items = state.items.filter((i) => i.id != data.Id)
    if (state.item)
      state.items.push(item)
    state.loaded = true
  }
};
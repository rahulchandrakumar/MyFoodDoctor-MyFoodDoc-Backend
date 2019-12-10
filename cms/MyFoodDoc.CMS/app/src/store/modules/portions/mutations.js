export default {
  setItems: (state, data) => {
    state.items = data
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
    state.items = state.items.filter((i) => i.id != data)
    state.loaded = true
  }
};
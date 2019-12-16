export default {
  setItems: (state, data) => {
    state.items = data.values
    state.total = data.total
    state.loaded = true
  }
};
export default {
  setItems: (state, data) => {
    state.totalUsers = data.fullUserHistory
    state.loaded = true
  }
};
export default {
  states(state) {
    return state.StateItems
  },
  addedItems(state) {
    return state.AddedItems
  },
  updatedItems(state) {
    return state.UpdatedItems
  },
  deletedItems(state) {
    return state.DeletedItems
  }
}
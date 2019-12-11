export default {
  setConnection: (state, data) => {
    state.connection = data
  },
  removeGroup: (state, data) => {
    delete state.StateItems[data.groupName]
    state.StateItems = Object.assign({}, state.StateItems)

    delete state.AddedItems[data.groupName]
    state.AddedItems = Object.assign({}, state.AddedItems)
    delete state.UpdatedItems[data.groupName]
    state.UpdatedItems = Object.assign({}, state.UpdatedItems)
    delete state.DeletedItems[data.groupName]
    state.DeletedItems = Object.assign({}, state.DeletedItems)
  },
  setStates: (state, data) => {
    state.StateItems[data.groupName] = data.stateItems
    state.StateItems = Object.assign({}, state.StateItems)

    state.AddedItems[data.groupName] = []
    state.AddedItems = Object.assign({}, state.AddedItems)
    state.UpdatedItems[data.groupName] = []
    state.UpdatedItems = Object.assign({}, state.UpdatedItems)
    state.DeletedItems[data.groupName] = []
    state.DeletedItems = Object.assign({}, state.DeletedItems)
  },
  addState: (state, data) => {
    state.StateItems[data.groupName][data.stateItem.Id] = data.stateItem
    state.StateItems = Object.assign({}, state.StateItems)
  },
  removeState: (state, data) => {
    delete state.StateItems[data.groupName][data.id]
    state.StateItems = Object.assign({}, state.StateItems)
  },
  itemAdded: (state, data) => {
    state.AddedItems[data.groupName].push(data.id)
    state.AddedItems = Object.assign({}, state.AddedItems)
  },
  itemUpdated: (state, data) => {
    state.UpdatedItems[data.groupName].push(data.id)
    state.UpdatedItems = Object.assign({}, state.UpdatedItems)
  },
  itemDeleted: (state, data) => {
    state.DeletedItems[data.groupName].push(data.id)
    state.DeletedItems = Object.assign({}, state.DeletedItems)
  }
}
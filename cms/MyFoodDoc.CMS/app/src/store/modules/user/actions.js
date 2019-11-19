import integration from "@/integration";

export default {
  login: async ({ commit }, { username, password }) => {
    let response = await integration.auth.login(username, password)
    if (response.status !== 200) {
      if (response.status === 400) {
        throw new Error("Wrong login/password");
      } else {
        throw new Error(`undefined error in backend (${response.status})`)
      }
    }
    commit("login", response.data)
    return true
  },
  logout: ({ commit }) => {
    commit("logout")
    return true
  }
};

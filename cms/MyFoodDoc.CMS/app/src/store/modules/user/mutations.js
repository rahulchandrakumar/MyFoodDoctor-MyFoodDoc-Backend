export default {
  login: (state, data) => {
    state.userInfo.isAuthenticated = true;
    state.userInfo.username = data.username;
    state.userInfo.roles = data.roles;
    state.token = data.token;

    localStorage.MyFoodDoc_roles = data.roles.join(",")
    localStorage.MyFoodDoc_username = data.username
    localStorage.access_token = data.token
  },
  logout: state => {
    state.userInfo.isAuthenticated = false;
    state.userInfo.username = "";
    state.userInfo.roles = [];
    state.token = "";

    localStorage.removeItem("access_token");
    localStorage.removeItem("MyFoodDoc_roles");
    localStorage.removeItem("MyFoodDoc_username");
  }
};

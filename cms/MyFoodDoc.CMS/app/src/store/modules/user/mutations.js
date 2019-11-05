export default {
  login: (state, data) => {
    state.userInfo.isAuthenticated = true;
    state.userInfo.username = data.userName;
    state.userInfo.roles = data.roles;
    state.token = data.token;

    sessionStorage.setItem("MyFoodDoc_roles", data.roles.join(","));
    sessionStorage.setItem("MyFoodDoc_username", data.userName);
    sessionStorage.setItem("access_token", data.token);
  },
  logout: state => {
    state.userInfo.isAuthenticated = false;
    state.userInfo.username = "";
    state.userInfo.roles = [];
    state.token = "";

    sessionStorage.removeItem("access_token");
    sessionStorage.removeItem("MyFoodDoc_roles");
    sessionStorage.removeItem("MyFoodDoc_username");
  }
};

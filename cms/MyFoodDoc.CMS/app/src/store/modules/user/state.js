const getDefaultState = function() {
  return {
    userInfo: {
      isAuthenticated: localStorage.access_token ? true : false,
      username: localStorage.MyFoodDoc_username || "",
      roles: (localStorage.MyFoodDoc_roles || "").split(",")
    },
    token: localStorage.access_token
  };
};

const state = getDefaultState();

export default state;

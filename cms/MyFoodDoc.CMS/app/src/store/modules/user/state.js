const getDefaultState = function() {
  return {
    userInfo: {
      isAuthenticated: sessionStorage.getItem("access_token") ? true : false,
      username: sessionStorage.getItem("vonoviaCMS_username") || "",
      roles: (sessionStorage.getItem("vonoviaCMS_roles") || "").split(",")
    },
    token: sessionStorage.getItem("access_token")
  };
};

const state = getDefaultState();

export default state;

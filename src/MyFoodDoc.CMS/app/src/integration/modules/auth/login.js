import httpRequest from "@/integration/httpRequest";

export default async function(username, password) {
  let result = await httpRequest.post("/api/v1/auth/login", {
    username,
    password,
    rememberMe: true
  });

  httpRequest.setHeader({ key: "Authorization", value: "bearer " + result.data.token })

  return result;
}

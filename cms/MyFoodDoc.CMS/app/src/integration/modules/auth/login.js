import httpRequest from "@/integration/httpRequest";

export default async function(username, password) {
  return await httpRequest.post("/api/v1/auth/login", {
    username,
    password,
    rememberMe: true
  });
}

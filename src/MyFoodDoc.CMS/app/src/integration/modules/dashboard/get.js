import httpRequest from "@/integration/httpRequest";

export default async function() {
  return await httpRequest.get("/api/v1/dashboard/");
}

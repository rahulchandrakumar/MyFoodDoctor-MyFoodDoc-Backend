import httpRequest from "@/integration/httpRequest";

export default async function(item) {
  return await httpRequest.post("/api/v1/portions", item);
}

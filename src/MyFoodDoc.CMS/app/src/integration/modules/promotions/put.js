import httpRequest from "@/integration/httpRequest";

export default async function(item) {
  return await httpRequest.put("/api/v1/promotions", item);
}

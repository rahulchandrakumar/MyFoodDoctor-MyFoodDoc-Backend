import httpRequest from "@/integration/httpRequest";

export default async function(id) {
  return await httpRequest.delete("/api/v1/promotions/"+id);
}

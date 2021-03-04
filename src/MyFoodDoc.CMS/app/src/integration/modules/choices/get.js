import httpRequest from "@/integration/httpRequest";

export default async function(id) {
    return await httpRequest.get("/api/v1/choices/" + id);
}

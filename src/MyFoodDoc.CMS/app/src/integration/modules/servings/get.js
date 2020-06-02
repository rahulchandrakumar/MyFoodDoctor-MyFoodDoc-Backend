import httpRequest from "@/integration/httpRequest";

export default async function (args) {
    return await httpRequest.get("/api/v1/servings", args);
}

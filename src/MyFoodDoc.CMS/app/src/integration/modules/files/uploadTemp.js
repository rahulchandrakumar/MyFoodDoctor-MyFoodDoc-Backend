import httpRequest from "@/integration/httpRequest";

export default async function(file) {
  let formData = new FormData();
  formData.append('file', file)

  var result = await httpRequest.post("api/v1/files/uploadTemp", formData, 'file');
  return result.data;
}
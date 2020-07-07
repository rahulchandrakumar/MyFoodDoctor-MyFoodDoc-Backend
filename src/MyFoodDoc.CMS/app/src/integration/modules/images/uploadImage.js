import httpRequest from "@/integration/httpRequest";
import { decode } from "base64-arraybuffer";

function _imageDecode (b64encoded) {
  let splitted = b64encoded.split(',')
  let decoded = decode(splitted[splitted.length - 1])
  return new Blob([decoded])
}

export default async function(base64img) {
  let formData = new FormData();
  formData.append('file', _imageDecode(base64img))

  var result = await httpRequest.post("/api/v1/images/upload", formData, 'file');
  return result.data;
}
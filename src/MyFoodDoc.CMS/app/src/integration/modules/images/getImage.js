import httpRequest from "@/integration/httpRequest";
import { encode } from "base64-arraybuffer";

function _imageEncode (arrayBuffer) {
  let b64encoded = encode(arrayBuffer)
  let mimetype="image/jpeg"
  return "data:"+mimetype+";base64,"+b64encoded
}

export default async function(url) {
  var result = await httpRequest.download(url);
  return _imageEncode(result.data);
}
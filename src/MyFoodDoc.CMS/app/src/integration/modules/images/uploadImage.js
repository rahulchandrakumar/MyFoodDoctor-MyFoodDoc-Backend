import httpRequest from "@/integration/httpRequest";
import { decode } from "base64-arraybuffer";

function _imageDecode(b64encoded) {
    let regexp = /data:(?<contenttype>.+?);base64,(?<data>.+)/

    let groups = b64encoded.match(regexp).groups

    let decoded = decode(groups.data)
    let blob = new Blob([decoded])

    if (groups.contenttype.startsWith('image/png')) {
        return new File([blob], 'image.png', {
            type: 'image/png'
        });
    } else {
        return new File([blob], 'image.jpg', {
            type: 'image/jpeg'
        });
    }
}

export default async function (base64img) {
    let formData = new FormData();
    formData.append('file', _imageDecode(base64img))

    var result = await httpRequest.post("/api/v1/images/upload", formData, 'file');
    return result.data;
}
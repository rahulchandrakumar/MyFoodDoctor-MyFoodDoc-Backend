import httpRequest from "@/integration/httpRequest";

export default async function(id) {
  var result = await httpRequest.download("api/v1/files/coupons", { id }, 'blob');
  
  var fileURL = window.URL.createObjectURL(new Blob([result.data]));
  var fileLink = document.createElement('a');
  fileLink.href = fileURL;
  fileLink.setAttribute('download', 'file.txt');
  document.body.appendChild(fileLink);
  fileLink.click();
}
import axios from "axios";

const contentTypes = {
  json: "application/json",
  form: "application/x-www-form-urlencoded",
  file: "multipart/form-data"
};

let axiosInstance = axios.create({
  baseURL: process.env.VUE_APP_WEB_API_URL,
  timeout: 100000
});

axiosInstance.interceptors.request.use(
  // Do something before request is sent
  config => {
    return config;
  },
  // Do something with request error
  error => {
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  // Do something with response data
  response => {
    return response;
  },
  // Do something with response error
  error => {
    return Promise.reject(error);
  }
);

class HttpRequest {
  constructor() {
    this.axios = axios;
  }

  setHeader(header) {
    axiosInstance.defaults.headers.common[header.key] = header.value;
    axiosInstance.defaults.headers.post["Content-Type"] =
      "application/x-www-form-urlencoded";
  }

  async get(path, data = null) {
    return await axiosInstance.get(path, { params: data });
  }

  async download(path) {
    return await axiosInstance.get(path, { responseType: 'arraybuffer' })
  }

  async post(path, data, type = "json") {
    return await axiosInstance.post(path, data, { "Content-Type": contentTypes[type] });
  }

  async put(path, data, type = "json") {
    return await axiosInstance.put(path, data, { "Content-Type": contentTypes[type] });
  }

  async delete(path, data) {
    return await axiosInstance.delete(path, { params: data });
  }
}

export default new HttpRequest();

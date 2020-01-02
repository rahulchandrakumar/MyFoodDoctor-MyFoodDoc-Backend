import axios from "axios";
import qs from "qs"
import Store from "@/store";
import router from '@/router'

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
    if (error.response.status === 401) {
      Store.dispatch("user/logout")
      router.push('/login')
    }
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

  getHeaders() {
    var headers = {}
    if (Store.state.user.token)
      headers["Authorization"] = "Bearer " + Store.state.user.token;

    return headers;
  }

  async get(path, data = null) {
    return await axiosInstance.get(path + (data == null ? '' : `?${qs.stringify(data, { allowDots: true })}`), { headers: this.getHeaders() });
  }

  async download(path, data = null, type = 'arraybuffer') {
    return await axiosInstance.get(path, { headers: this.getHeaders(), params: data, responseType: type })
  }

  async post(path, data, type = "json") {
    return await axiosInstance.post(path, data, { headers: this.getHeaders(), "Content-Type": contentTypes[type] });
  }

  async put(path, data, type = "json") {
    return await axiosInstance.put(path, data, { headers: this.getHeaders(), "Content-Type": contentTypes[type] });
  }

  async delete(path, data) {
    return await axiosInstance.delete(path, { headers: this.getHeaders(), params: data });
  }
}

export default new HttpRequest();

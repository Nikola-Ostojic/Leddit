import axios from "axios";

import { apiUrl } from "config/constants";

const CancelToken = axios.CancelToken;
export const cancelTokenSource = CancelToken.source();

const axiosInstance = axios.create({
  baseURL: apiUrl,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  }
  // validateStatus: (status) => true
});

export const apiCall = ({ url, method, data, requiresAuth = false }) =>
  axiosInstance({
    method,
    url,
    data,
    requiresAuth
  });
export default axiosInstance;

import axios from "axios";
import { backendUrl } from "../config/config";

const HTTP = axios.create({ baseURL: backendUrl });

HTTP.interceptors.request.use(
  (config) => {
    let accessToken = "";
    if (localStorage.getItem("accessToken") != undefined) {
      accessToken = localStorage.getItem("accessToken");
    }
    config.headers = {
      Accept: "application/json",
      "Content-Type": "application/json",
      Authorization: `Bearer ${accessToken}`,
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods": "GET, POST, PATCH, PUT, DELETE, OPTIONS",
      "Access-Control-Allow-Headers": "Origin, Content-Type, X-XSRF-TOKEN",
    };
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);
axios.interceptors.response.use(undefined, function (error) {
  if (error) {
    const originalRequest = error.config;
    if (
      (error.response.status === 401 || error.response.status === 403) &&
      !originalRequest._retry
    ) {
      originalRequest._retry = true;
    }
  }
  return Promise.reject(error);
});

export function GET(url, queryPayload = {}) {
  return HTTP.get(`${backendUrl}${url}`, { params: queryPayload });
}

export function POST(url, body) {
  return HTTP.post(`${backendUrl}${url}`, body);
}

export function PUT(url, body) {
  return HTTP.put(`${backendUrl}${url}`, body);
}

export function DELETE(url, queryPayload = {}) {
  return HTTP.delete(`${backendUrl}${url}`, { params: queryPayload });
}

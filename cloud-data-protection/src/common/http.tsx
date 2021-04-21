import AuthInterceptor from "./interceptors/auth";

const axios = require('axios').default;

const a = new AuthInterceptor();

axios.defaults.baseURL = process.env.REACT_APP_API_BASEURL;

axios.interceptors.request.use(a.intercept);

export const http = axios;
import authInterceptor from "./interceptors/auth";

const axios = require('axios').default;

axios.defaults.baseURL = process.env.REACT_APP_API_BASEURL;

axios.interceptors.request.use(authInterceptor);

export const http = axios;
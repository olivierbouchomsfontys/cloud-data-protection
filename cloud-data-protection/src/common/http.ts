import AuthInterceptor from "./interceptors/auth";

const axios = require('axios').default;

const authInterceptor = new AuthInterceptor();

axios.defaults.baseURL = process.env.REACT_APP_API_BASEURL;
axios.defaults.headers.redirect = 'follow';

axios.interceptors.request.use(authInterceptor.intercept);

export const http = axios;
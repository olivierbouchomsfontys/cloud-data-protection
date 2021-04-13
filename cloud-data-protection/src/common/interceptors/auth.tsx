import {AxiosRequestConfig} from "axios";

const authInterceptor = (config: AxiosRequestConfig) => {
    return config;
}

export default authInterceptor;
import {AxiosRequestConfig} from "axios";
import store from 'stores/Store';

class AuthInterceptor {
    public intercept(config: AxiosRequestConfig): Promise<AxiosRequestConfig> {
        const token = store.getState().user.token;

        if (token) {
            config.headers['Authorization'] = 'Bearer ' + token;
        }

        return Promise.resolve(config);
    }
}

export default AuthInterceptor;
import store from 'stores/Store';
import {http} from "common/http";
import {login, logout} from "features/userSlice";
import {ServiceBase} from "./base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {LoginInput} from "./input/loginInput";
import {LoginResult} from "./result/loginResult";
import {RegisterInput} from "./input/registerInput";

export class AuthService extends ServiceBase {
    public async login(input: LoginInput, cancelToken?: CancelToken) {
        await http.post('/Authentication/Authenticate', input, { cancelToken: cancelToken })
            .then((response: AxiosResponse<LoginResult>) => AuthService.doLogin(response))
            .catch((e: any) => this.onError(e));
    }

    public async register(input: RegisterInput, cancelToken?: CancelToken) {
        await http.post('/Authentication/Register', input, { cancelToken })
            .catch((e: any) => this.onError(e));
    }

    public logout() {
        store.dispatch(logout());
    }

    private static doLogin(response: AxiosResponse<LoginResult>) {
        store.dispatch(login(response.data));
    }
}

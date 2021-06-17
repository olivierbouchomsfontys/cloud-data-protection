import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {logout} from "features/userSlice";
import {http} from "common/http";
import store from "stores/Store";
import ChangeEmailInput from "./input/account/changeEmailInput";

export class AccountService extends ServiceBase {
    public async changeEmail(input: ChangeEmailInput, cancelToken?: CancelToken) {
        return await http.patch('/Account/Email', input, { cancelToken: cancelToken })
            .catch((e: any) => this.onError(e));
    }

    public async delete(cancelToken?: CancelToken) {
        return await http.delete('/Account', { cancelToken: cancelToken })
            .then((response: AxiosResponse) => AccountService.onDelete(response))
            .catch((e: any) => this.onError(e));
    }

    private static onDelete(response: AxiosResponse) {
        store.dispatch(logout());
    }
}
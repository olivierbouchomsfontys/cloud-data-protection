import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {logout, changeEmail} from "features/userSlice";
import {http} from "common/http";
import store from "stores/Store";
import ChangeEmailInput from "./input/account/changeEmailInput";
import ConfirmChangeEmailInput from "services/input/account/confirmChangeEmailInput";
import ConfirmChangeEmailResult from "services/result/account/confirmChangeEmailResult";
import ChangePasswordInput from "services/input/account/changePasswordInput";

export class AccountService extends ServiceBase {
    public async changeEmail(input: ChangeEmailInput, cancelToken?: CancelToken) {
        return await http.patch('/Account/Email', input, { cancelToken: cancelToken })
            .catch((e: any) => this.onError(e));
    }

    public async confirmChangeEmail(input: ConfirmChangeEmailInput, cancelToken?: CancelToken) {
        return await http.patch('/Account/ConfirmEmail', input, { cancelToken: cancelToken })
            .then((response: AxiosResponse<ConfirmChangeEmailResult>) => AccountService.onConfirmChangeEmail(response))
            .catch((e: any) => this.onError(e));
    }

    public async changePassword(input: ChangePasswordInput, cancelToken?: CancelToken) {
        return await http.patch('/Account/ChangePassword', input, { cancelToken: cancelToken })
            .catch((e: any) => this.onError(e));
    }

    public async delete(cancelToken?: CancelToken) {
        return await http.delete('/Account', { cancelToken: cancelToken })
            .then((response: AxiosResponse) => AccountService.onDelete(response))
            .catch((e: any) => this.onError(e));
    }

    private static onConfirmChangeEmail(response: AxiosResponse<ConfirmChangeEmailResult>): Promise<ConfirmChangeEmailResult> {
        store.dispatch(changeEmail(response.data));

        return Promise.resolve(response.data);
    }

    private static onDelete(response: AxiosResponse) {
        store.dispatch(logout());
    }
}
import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {logout} from "features/userSlice";
import {http} from "common/http";
import store from "stores/Store";

export class AccountService extends ServiceBase {
    public async delete(cancelToken?: CancelToken) {
        return await http.delete('/Account', { cancelToken: cancelToken })
            .then((response: AxiosResponse) => AccountService.onDelete(response))
            .catch((e: any) => this.onError(e));
    }

    private static onDelete(response: AxiosResponse) {
        store.dispatch(logout());
    }
}
import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import BackupSchemeResult from "services/result/backupScheme/backupSchemeResult";
import {http} from "common/http";

class BackupSchemeService extends ServiceBase {
    public async get(cancelToken?: CancelToken): Promise<BackupSchemeResult[]> {
        return await http.get('/BackupScheme', { cancelToken })
            .then((response: AxiosResponse<BackupSchemeResult[]>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default BackupSchemeService;
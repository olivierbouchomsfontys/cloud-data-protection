import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import BackupConfigurationResult from "services/result/backupConfiguration/backupConfigurationResult";
import {http} from "common/http";
import CreateBackupConfigurationResult from "services/result/backupConfiguration/createBackupConfigurationResult";
import CreateBackupConfigurationInput from "services/input/backupConfiguration/createBackupConfigurationInput";

class BackupConfigurationService extends ServiceBase {
    public async get(cancelToken?: CancelToken): Promise<BackupConfigurationResult> {
        return await http.get('/BackupConfiguration', { cancelToken })
            .then((response: AxiosResponse<BackupConfigurationResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async create(data: CreateBackupConfigurationInput, cancelToken?: CancelToken): Promise<CreateBackupConfigurationResult> {
        return await http.post('/BackupConfiguration', data, { cancelToken })
            .then((response: AxiosResponse<CreateBackupConfigurationResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default BackupConfigurationService;
import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {http} from "common/http";
import FileUploadResult from "services/result/demo/fileUploadResult";

class DemoService extends ServiceBase {
    public static readonly maxFileSize: number = 25 * 1024 * 1024;

    public async upload(file: File, cancelToken?: CancelToken): Promise<FileUploadResult> {
        const formData = new FormData();

        formData.append('File', file);

        return await http.post('/demo/file', formData, { cancelToken: cancelToken, headers: { 'content-type': 'multipart/form-data' } })
            .then((response: AxiosResponse<FileUploadResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default DemoService;
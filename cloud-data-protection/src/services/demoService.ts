import {ServiceBase} from "services/base/serviceBase";
import {CancelToken} from "axios";
import {http} from "common/http";

class DemoService extends ServiceBase {
    public static readonly maxFileSize: number = 25 * 1024 * 1024;

    public async upload(file: File, cancelToken?: CancelToken) {
        const formData = new FormData();

        formData.append('File', file);

        await http.post('/demo/file', formData, { cancelToken: cancelToken, headers: { 'content-type': 'multipart/form-data' } });
    }
}

export default DemoService;
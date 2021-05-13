import {ServiceBase} from "services/base/serviceBase";
import {AxiosResponse, CancelToken} from "axios";
import {http} from "common/http";
import FileUploadResult from "services/result/demo/fileUploadResult";
import FileInfoResult from "services/result/demo/fileInfoResult";
import {FileDownloadResult} from "services/result/demo/fileDownloadResult";
import {readFileName} from "common/parser/contentDisposition";
import fileDownload from "js-file-download";

class DemoService extends ServiceBase {
    public static readonly maxFileSize: number = 25 * 1024 * 1024;

    public async upload(file: File, cancelToken?: CancelToken): Promise<FileUploadResult> {
        const formData = new FormData();

        formData.append('File', file);

        return await http.post('/demo/file', formData, { cancelToken: cancelToken, headers: { 'content-type': 'multipart/form-data' } })
            .then((response: AxiosResponse<FileUploadResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async getFileInfo(id: string, cancelToken?: CancelToken): Promise<FileInfoResult> {
        return await http.get('/demo/file', { cancelToken: cancelToken, params: { id: id } })
            .then((response: AxiosResponse<FileInfoResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async downloadFile(id: string, decrypt: boolean, cancelToken?: CancelToken): Promise<FileDownloadResult | undefined> {
        let headers;
        let responseType;

        if (decrypt) {
            headers = { 'accept' : 'application/octet-stream'}
            responseType = 'blob';
        } else {
            headers = { 'accept' : 'application/json'}
            responseType = 'json';
        }


        return await http.get('/demo/file/download', { cancelToken: cancelToken, params: { id: id, decrypt: decrypt }, headers: headers, responseType: responseType })
            .then((response: AxiosResponse) => {
                if (DemoService.shouldDownload(response)) {
                    fileDownload(response.data, readFileName(response.headers['content-disposition']), response.data.contentType);
                    return Promise.resolve(undefined);
                }

                return response.data as FileDownloadResult;
            })
            .catch((e: any) => this.onError(e));
    }

    private static shouldDownload(response: AxiosResponse): boolean {
        return response.headers['content-disposition'];
    }
}

export default DemoService;
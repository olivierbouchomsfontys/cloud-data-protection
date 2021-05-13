export interface FileDownloadResult {
    url: string;
    type: FileDownloadResultType;
    bytes: any;
    fileName: string;
}

export enum FileDownloadResultType {
    Bytes = 0,
    Url = 1
}


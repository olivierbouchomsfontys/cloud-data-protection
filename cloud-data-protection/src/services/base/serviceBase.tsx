export abstract class ServiceBase {
    protected onError(error: any) {
        if (!error?.response?.data) {
            return ServiceBase.unknownError();
        }

        // 401 unauthorized
        if (error.response.data.statusDescription && error.response.data.message) {
            return Promise.reject(error.response.data.statusDescription + ': ' + error.response.data.message.toLowerCase());
        }

        if (error.response.data) {
            return Promise.reject(error.response.data);
        }

        return ServiceBase.unknownError();
    }

    private static unknownError(): Promise<any> {
        return Promise.reject('An unknown error has occurred.')
    }
}
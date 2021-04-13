export abstract class ServiceBase {
    protected onError(error: any) {
        if (!error?.response?.data?.statusDescription || !error.response.data.message) {
            return Promise.reject('An unknown error has occurred.')
        }

        return Promise.reject(error.response.data.statusDescription + ': ' + error.response.data.message.toLowerCase());
    }
}
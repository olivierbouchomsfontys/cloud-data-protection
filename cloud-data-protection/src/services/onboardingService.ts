import {ServiceBase} from "services/base/serviceBase";
import {http} from "common/http";
import {AxiosResponse, CancelToken} from "axios";
import OnboardingResult from "services/result/onboarding/onboardingResult";

class OnboardingService extends ServiceBase {
    public static googleLoginUrl: string = http.defaults.baseURL + '/Onboarding/GoogleLogin';

    public async get(cancelToken?: CancelToken): Promise<OnboardingResult> {
        return await http.get('/Onboarding', { cancelToken })
            .then((response: AxiosResponse<OnboardingResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default OnboardingService;
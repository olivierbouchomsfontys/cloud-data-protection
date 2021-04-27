import {ServiceBase} from "services/base/serviceBase";
import {http} from "common/http";
import {AxiosResponse, CancelToken} from "axios";
import OnboardingResult from "services/result/onboardingResult";

class OnboardingService extends ServiceBase {
    public async get(cancelToken?: CancelToken): Promise<OnboardingResult> {
        return await http.get('/Onboarding', { cancelToken })
            .then((response: AxiosResponse<OnboardingResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default OnboardingService;
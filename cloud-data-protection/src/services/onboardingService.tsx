import {ServiceBase} from "./base/serviceBase";
import {http} from "common/http";
import {AxiosResponse, CancelToken} from "axios";
import OnboardingResult from "./result/onboardingResult";
import Onboarding from "entities/onboarding";

class OnboardingService extends ServiceBase {
    public async get(cancelToken?: CancelToken): Promise<Onboarding> {
        return await http.get('/Onboarding', { cancelToken })
            .then((response: AxiosResponse<OnboardingResult>) => response.data as Onboarding)
            .catch((e: any) => this.onError(e));
    }
}

export default OnboardingService;
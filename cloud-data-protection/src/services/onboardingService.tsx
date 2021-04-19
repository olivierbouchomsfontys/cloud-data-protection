import {ServiceBase} from "./base/serviceBase";
import {http} from "common/http";
import {AxiosResponse} from "axios";
import OnboardingResult from "./result/onboardingResult";
import Onboarding from "entities/onboarding";

class OnboardingService extends ServiceBase {
    public async get(): Promise<Onboarding> {
        return await http.get('/Onboarding')
            .then((response: AxiosResponse<OnboardingResult>) => response.data as Onboarding);
    }
}

export default OnboardingService;
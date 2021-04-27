import OnboardingStatus from "entities/onboardingStatus";
import GoogleLoginInfoResult from "services/input/onboarding/googleLoginInfoResult";

interface OnboardingResult {
    id: number;
    userId: number;
    created: Date;
    status: OnboardingStatus;
    loginInfo: GoogleLoginInfoResult;
}

export default OnboardingResult;
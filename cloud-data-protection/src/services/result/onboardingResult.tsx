import OnboardingStatus from "../../entities/onboardingStatus";

interface OnboardingResult {
    id: number;
    userId: number;
    created: Date;
    status: OnboardingStatus;
}

export default OnboardingResult;
import OnboardingStatus from "./onboardingStatus";

interface Onboarding {
    id: number;
    userId: number;
    created: Date;
    status: OnboardingStatus;
}

export default Onboarding;
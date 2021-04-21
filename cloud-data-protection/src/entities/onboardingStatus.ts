enum OnboardingStatus {
    // No steps have been completed
    None = 0,

    // Account (e.g. Google Workspace) is connected
    AccountConnected = 10,

    // Onboarding is completed
    Complete = 1000
}

export default OnboardingStatus;
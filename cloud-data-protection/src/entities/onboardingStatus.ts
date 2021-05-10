enum OnboardingStatus {
    // No steps have been completed
    None = 0,

    // Account (e.g. Google Workspace) is connected
    AccountConnected = 10,

    // Backup scheme is set
    SchemeEntered = 20,

    // Onboarding is completed
    Complete = 1000
}

export default OnboardingStatus;
# Docker

Each runnable project has its own Dockerfile. An image can be built by running the following command, replacing project with the actual project you want to build.

Run the build script from the root ASP .NET Core directory. This is the same as the location of this README.

```
docker build . -f ./{project}/Dockerfile -t your_tag
```

Example for OnboardingService:

```
docker build . -f ./CloudDataProtection.Services.OnboardingService/Dockerfile -t cdp_onboarding_service
```
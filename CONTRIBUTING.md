# Contribution guidelines

## Adding secrets

The application must have access to credentials when integrating an external service that requires authentication, such as a database or REST-API. There are two ways to access these credentials.

### Appsettings

Use appsettings when the credentials can be public for a development environment. It doesn't harm and makes it easier to get started when these credentials are public. Examples are the JWT secret or a database connection string. The CI/CD-pipeline will handle injection of these credentials in the test environments. Use the `Options` pattern to inject and access secrets in the codebase.

### Environment variables

Use environment variables when the credentials should be secret for a development environment. These credentials are often personal accounts of a developer. Examples are authentication tokens for Sendgrid and client IDs and secrets. Use a `CredentialsProvider` with the `EnvironmentVariableHelper` to inject and access the secrets in the codebase.
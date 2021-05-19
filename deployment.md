# Services

| Name                  | Port  | External |
| -------------         | ----- | --- |
| Frontend              | 80    | ✅ |
| Gateway               | 5001  | ✅ |
| BackupConfiguration   | 5031  | ❌ |
| Onboarding            | 5021  | ❌ |
| Mail                  | 5051  | ❌ |

# Secrets

## Kubernetes

| Name in code                                      | Name in k8s                                   |
| -------------                                     | -----                                         |
| CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID        | cdp-onboarding-google-oauth2-client-id        |
| CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET    | cdp-onboarding-google-oauth2-client-secret    |
| CDP_DEV_SENDGRID                                  | cdp-sendgrid                                  |
| CDP_DEV_SENDGRID_SENDER                           | cdp-sendgrid-sender                           |

## Functions app

| Name in code                                      |
| --- |
| CDP_DEMO_BLOB_AES_IV                              |
| CDP_DEMO_BLOB_AES_KEY                             |
| CDP_DEMO_BLOB_CONNECTION                          |

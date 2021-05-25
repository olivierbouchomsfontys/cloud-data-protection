# Services

## Kubernetes

| Name                  | Port  | External |
| -------------         | ----- | --- |
| Frontend              | 80    | ✅ |
| Gateway               | 5001  | ✅ |
| BackupConfiguration   | 5031  | ❌ |
| Onboarding            | 5021  | ❌ |
| Mail                  | 5051  | ❌ |

## Functions

| Name | Location   |
| ---  | ---        |
| BackupDemo | ❓ |

# Secrets

## Kubernetes

| Name in code                                      | Name in k8s                                   |
| -------------                                     | -----                                         |
| CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID        | cdp-onboarding-google-oauth2-client-id        |
| CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET    | cdp-onboarding-google-oauth2-client-secret    |
| CDP_DEV_SENDGRID                                  | cdp-sendgrid                                  |
| CDP_DEV_SENDGRID_SENDER                           | cdp-sendgrid-sender                           |
| CDP_BACKUP_DEMO_FUNCTIONS_KEY                     | cdp-backup-demo-api-key                       |

## Functions

| Name in code              | Name in function          |
| ---                       | ---                       |
| CDP_DEMO_BLOB_AES_IV      | CDP_DEMO_BLOB_AES_IV      |
| CDP_DEMO_BLOB_AES_KEY     | CDP_DEMO_BLOB_AES_KEY     |
| CDP_DEMO_BLOB_CONNECTION  | CDP_DEMO_BLOB_CONNECTION  |
| CDP_BACKUP_DEMO_API_KEY   | CDP_BACKUP_DEMO_API_KEY   |
# Adding endpoints in Ocelot

Add a route to the `ReRoutes` property in `Ocelot.json`.

Example without authentication:

```json
    {
      "DownstreamPathTemplate": "/onboarding",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5021
        }
      ],
      "UpstreamPathTemplate": "/onboardinganonymous",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT" ],
      "DangerousAcceptAnyServerCertificateValidator": true
    }
```

Example with authentication - notice the `AuthenticationOptions` property:

```json
    {
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      },
      "DownstreamPathTemplate": "/onboarding",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5021
        }
      ],
      "UpstreamPathTemplate": "/onboarding",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT" ],
      "DangerousAcceptAnyServerCertificateValidator": true
    }
```
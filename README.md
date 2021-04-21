# Cloud Data Protection

Build status: [![Build Status](https://dev.azure.com/OlivierBouchomsFontys/CloudDataProtection/_apis/build/status/CloudDataProtection%20master?branchName=master)](https://dev.azure.com/OlivierBouchomsFontys/CloudDataProtection/_build/latest?definitionId=2&branchName=master)

This repository contains all the code for the Cloud Data Protection project, made for Fontys Hogeschool ICT.

Author: Olivier Bouchoms

Email: o.bouchoms@student.fontys.nl

Branches:

* Master: production environment (n.a.)
* Staging: staging environment (n.a.)
* Test: test environment (will be set up during the semester)
* Develop: development branch

# How to run locally

## SendGrid

SendGrid is used to send mails in the development environment. Create a user account and API key on SendGrid and store the API key in an environment variable.

Unix:
`export CDP_DEV_SENDGRID={your api key goes here}`

Windows:
`setx CDP_DEV_SENDGRID {your api key goes here}`

You also need to verify your email address to send mails, see https://sendgrid.com/docs/ui/sending-email/sender-verification/. Then it should be set as an environment variable.

Unix:
`export CDP_DEV_SENDGRID_SENDER={your email goes here}`

Windows:
`setx CDP_DEV_SENDGRID_SENDER {your email goes here}`

## Database and messagebroker

Navigation to the `CloudDataProtection` directory and run `./dev.sh`. This will create the required containers for the PostgreSQL database and RabbitMq messagebroker.

## Logging in

A client account can be created by registering a new account. It is not possible yet to change the password of the default (employee) user. An alternative is to change it by replacing the password hash in the database with your own.

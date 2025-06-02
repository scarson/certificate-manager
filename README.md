# certificate-manager
A web app to track X.509 certificates and send expiration and renewal reminders. A learning project for ASP.NET Core, Angular, and AI-assisted coding.

The app should be cross-platform and run on Windows or Linux. It should be easy to containerize.

The app should be able to:
- Import a certificate from the following sources:
  - File
  - URL
  - Local machine via thumbprint
- Allow optional indication if the certificate is used for server or client authentication
- Track the certificate's expiration date
- Send an email reminder via SMTP when:
  - The certificate is about to expire
  - The certificate is expiring
- Support both SMTP basic authentication and OAuth 2.0 authentication
- Support multiple user authentication methods:
    - Email and password (local account)
    - OAuth 2.0 with:
        - Google
        - Microsoft
    - SAML with:
        - Microsoft Entra ID
    - Platform authentication with:
        - Windows
            - Members of the local Administrators group
        - Linux
            - Members of the sudo group
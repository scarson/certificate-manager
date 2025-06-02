# Certificate Manager

A web application to track X.509 certificates and send expiration and renewal reminders. Built with ASP.NET Core, Blazor WebAssembly, and SQLite.

## Features

- **Certificate Management**
  - Import certificates from multiple sources:
    - File upload
    - URL
    - Local machine (via thumbprint)
  - Track certificate expiration dates
  - Categorize certificates by usage (server/client authentication)

- **Notifications**
  - Email reminders for upcoming expirations
  - Configurable notification thresholds
  - Support for SMTP with both basic and OAuth 2.0 authentication

- **Authentication & Authorization**
  - Local accounts (email/password)
  - OAuth 2.0 providers:
    - Google
    - Microsoft
  - SAML 2.0 (Microsoft Entra ID)
  - Platform authentication:
    - Windows (Local Administrators group)
    - Linux (sudo group)

## Project Structure

```
certificate-manager/
├── src/
│   ├── CertificateManager.Server/    # ASP.NET Core Web API
│   └── CertificateManager.Client/    # Blazor WebAssembly frontend
└── tests/
    └── (Test projects will be added here)
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/) (Included with most operating systems)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (Recommended) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/certificate-manager.git
   cd certificate-manager
   ```

2. **Restore .NET packages**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run --project src/CertificateManager.Server
   ```

The application will be available at `https://localhost:5001`.

## Development

### Building the Solution
```bash
dotnet build
```

### Running Tests
Test projects will be added as development progresses.

### Code Style
This project follows the [.NET Coding Style](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) and uses [EditorConfig](https://editorconfig.org/) for consistent code formatting.

## Deployment

The application can be published using:

```bash
dotnet publish -c Release -o ./publish
```

Or containerized using the provided Dockerfile:

```bash
docker build -t certificate-manager .
docker run -p 5000:80 certificate-manager
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
# planthor-identity-server-openiddict

Migrate IDP to OpenIddict solution

## Setup

### Prerequisite

- .NET 10.0 SDK
- Dotnet IDE of your choice (recommend VSCode)
- Docker {Docker Desktop/Podman Desktop}

### Prepare your first run

- Install required tools

```bash
dotnet tool restore
```

- Build and Test check

```bash
dotnet clean; dotnet restore; dotnet build; dotnet test
```

- Update Facebook Authentication environment variable:
  - Option #1: Update the appsetting.Development.json value directly.
  - Option #2: Run the bash script

```bash
$env:Authentication__Facebook__ClientId = "YOUR_CLIENT_ID_HERE";
$env:Authentication__Facebook__ClientSecret = "YOUR_CLIENT_SECRET_HERE";
```

#### Local Code Coverage

- Use Coverlet

```bash
dotnet test --collect:"XPlat Code Coverage";
```

- Generate report

```bash
dotnet reportgenerator -reports:".\TestResults\{Guid}\coverage.cobertura.xml" -targetdir:".\TestResults\{Guid}\ReportGenerator"
```

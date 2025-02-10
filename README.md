# Golub - Smart Email Relay Service

Golub is a lightweight email relay service built with .NET 9. It intelligently rotates between multiple email providers (such as SendGrid, Brevo, and others) to maximize free-tier email sending before switching to the next available provider.

## Features

- **Smart Provider Rotation** – Uses free-tier quotas before switching to another provider.
- **API Key Management** – API keys are securely stored in a database.
- **Configurable Rate Limiting** – Sending limits are dynamically adjusted based on available provider quotas.
- **Self-Hosted** – Runs on Linux (Ubuntu) or any environment that supports .NET 9.
- **Open-Source Contributions** – Fork, improve, and submit PRs!

## Installation & Setup

### Prerequisites
- .NET 9 SDK
- PostgreSQL or any supported database for storing API keys and provider settings
- Linux server (for deployment) or Windows/Mac for local development

### Running Locally
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/golub.git
   cd golub
   ```
2. Configure your database connection in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=golub;Username=youruser;Password=yourpassword"
     },
     "EmailProviders": {
       "SendGrid": { "ApiKey": "your-sendgrid-key" },
       "Brevo": { "ApiKey": "your-brevo-key" }
     }
   }
   ```
3. Apply migrations:
   ```sh
   dotnet ef database update
   ```
4. Run the service:
   ```sh
   dotnet run
   ```

## API Endpoints

### Send Email
**POST** `/api/emails/send`

#### Request Body:
```json
{
  "to": "recipient@example.com",
  "subject": "Hello from Golub!",
  "body": "This is a test email."
}
```

#### Response:
```json
{
  "success": true,
  "message": "Email sent successfully via SendGrid"
}
```

---

### Disable API Key
**POST** `/api/api-keys/disable`

#### Request Body:
```json
{
  "apiKey": "your-api-key"
}
```

#### Response:
```json
{
  "success": true,
  "message": "API key disabled."
}
```

## Configuration
- **Email Providers:** Settings are stored in the database but service-level configurations remain in `appsettings.json`.
- **Rate Limits:** The service dynamically adjusts based on available quotas for each provider.

## Deployment (Ubuntu Linux)

1. Publish the application:
   ```sh
   dotnet publish -c Release -o out
   ```
2. Move to your server and run:
   ```sh
   cd /path/to/out
   dotnet Golub.dll
   ```

For a production-ready setup, consider running Golub as a **systemd service** or inside a **Docker container**.

## Contributing
We welcome contributions! Feel free to fork, improve, and submit a pull request.

## License
MIT License - Free for use and modification.

---
Happy emailing with Golub! 🕊️
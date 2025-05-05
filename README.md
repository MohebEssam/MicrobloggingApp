
# 🐦 MicrobloggingApp

A lightweight Twitter/X-like microblogging platform built with modern web technologies including ASP.NET Core 8, Azure services, and MVC architecture.

---

## 📦 Technologies Used

- **ASP.NET Core 8 Web API** – Backend for managing posts, authentication, and user timelines.
- **ASP.NET Core MVC** – Frontend interface for user interaction.
- **Entity Framework Core (SQL Server)** – ORM for data access and schema management.
- **Azure Blob Storage** – Cloud-based storage for user-uploaded media (images, etc.).
- **Azure Queue Storage** – Handles background image processing and other async tasks.
- **Azure Application Insights** – Centralized performance and error monitoring.
- **Event Viewer Logging** – Local machine logging for deeper diagnostics.
- **OAuth 2.0 + JWT Authentication** – Secure authentication using token-based access.
- **xUnit** – Unit and integration test coverage for controllers and services.

---

## 🚀 How to Build and Run Locally

### 1. 📁 Clone the Repository
```bash
git clone https://github.com/MohebEssam/MicrobloggingApp.git
cd MicrobloggingApp
```

### 2. 🛠️ Set Up the Database

- Ensure **SQL Server** is running locally or remotely.
- Update the `appsettings.json` connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MicrobloggingApp;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

- Run database migrations:
```bash
dotnet ef database update
```

### 3. 🔐 Configure Secrets (if needed)

Add your `JwtOptions`, Azure Blob/Queue credentials, and other secrets either in:
- `appsettings.Development.json`
- Or via [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

Example snippet:
```json
"JwtOptions": {
  "Issuer": "MicrobloggingApp",
  "Audience": "MicrobloggingAppUser",
  "SecretKey": "YourSecureSecretKeyHere",
  "TokenDuration": "60"
},
"Azure": {
  "BlobStorageConnection": "<your-blob-connection>",
  "QueueConnection": "<your-queue-connection>"
}
```

### 4. 🏃‍♂️ Run the App

```bash
dotnet build
dotnet run --project MicrobloggingApp.API
dotnet run --project MicrobloggingApp.MVC
```

Visit `https://localhost:7056` for the frontend and `https://localhost:7251/api` for the API.

---

## 🧪 Testing

Run unit tests using:

```bash
dotnet test
```

---

## 📊 Monitoring & Logging

- **Azure Application Insights**: Configured for performance metrics, exceptions, and live telemetry.
- **Event Viewer Logging** (for local/IIS hosting):  
  Ensure the App Pool identity has permission to write to the Event Log.  
  You can also run the app as Administrator once to register the source.

---

## ⚙️ Architecture & Design Decisions

### ✅ Why These Components?

- **SQL Server**: Chosen for its maturity, integration with EF Core, and support for relational data integrity.  
  *Alternative considered*: PostgreSQL (also strong, but SQL Server was preferred due to familiarity and out-of-box .NET tooling).

- **Azure Blob Storage**: Scalable and cost-effective storage for large, unstructured files like images.  
  *Alternative considered*: Amazon S3, but Azure was selected due to tighter .NET integration.

- **Azure Queue Storage**: Ensures loosely coupled, asynchronous background processing (e.g., image resizing).  
  *Alternative considered*: RabbitMQ or Azure Service Bus, but Queue Storage was simpler and sufficient for small scale.

- **JWT Authentication**: Enables stateless, scalable authentication ideal for APIs and SPAs.  
  *Alternative considered*: Cookie-based auth (more stateful and less API-friendly).

- **xUnit**: Community-preferred unit test framework in .NET Core with strong mocking ecosystem.

---

## 📫 Contact

Feel free to fork, contribute, or raise issues!  
Maintained by [Moheb Essam](https://github.com/MohebEssam).

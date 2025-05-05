# MicrobloggingApp

A small microblogging application (like Twitter/X) built with .NET 8, Azure Blob Storage, and MVC Frontend.

---

## 📦 Technologies
- ASP.NET Core 8 Web API
- ASP.NET Core MVC Frontend
- Entity Framework Core (SQL Server)
- Azure Blob Storage
- OAuth 2.0 + JWT Authentication
- xUnit for Unit & Integration Tests
- Azure Function for background processing
- Azure Application Insights & EventViewer Logging
---	If you're deploying to IIS:
Set App Pool identity to a user that has permission to write to the Event Log.
Or run the app as admin at least once to allow it to create the Event Source.

---

## 🚀 How to Build and Run Locally

### 1. Clone the Repository
```bash
git clone https://your-repo-url.git
cd MicrobloggingApp

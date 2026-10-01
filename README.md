# HelpDesk System

A help desk management application built with ASP.NET Core MVC, Entity Framework Core, and ASP.NET Core Identity. The solution supports ticket creation, department and category management, priorities, comments, notifications, history tracking, and email notifications.

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- MailKit for email integration
- ClosedXML for Excel-related functionality

## Solution Structure

- HelpDesk.Presentation - MVC web application
- HelpDesk.BAL - Business logic and service layer
- HelpDesk.DAL - Data access layer, repositories, and DbContext

## Features

- Role-based access for Admin, Employee, and Support Engineer users
- Ticket creation, assignment, updates, and lifecycle tracking
- Department and category management for organization-wide support workflows
- Priority-based ticket classification and reporting
- Comment threads and ticket history for traceability
- Notifications and email alerts for ticket activity
- Excel export for filtered ticket reports and analytics dashboards

## Screenshots

### Login and Registration

![Login Screen](HelpDesk.Presentation/wwwroot/uploads/8670c17d-31eb-4417-811a-8b43cef69078.png)

### Admin Dashboard

![Admin Dashboard](HelpDesk.Presentation/wwwroot/uploads/4afe961a-1a3f-4fc7-a127-03b1ee830d86.png)

### Support Tickets

![Support Tickets](HelpDesk.Presentation/wwwroot/uploads/38bda361-3867-4e6e-b952-a7f53a276b31.jpg)

### Department and Category Management

![Departments](HelpDesk.Presentation/wwwroot/uploads/f93daa55-75fd-4252-ac9a-6da97157edfe.jpg)

![Categories](HelpDesk.Presentation/wwwroot/uploads/463900cf-4194-4f9d-a699-febfd65b5d59.jpg)

### Reports and Analytics

![Reports](HelpDesk.Presentation/wwwroot/uploads/8f270d1d-028a-4c49-a06f-faa1764a7032.jpg)

### Ticket Details and Support Flow

![Ticket Details](HelpDesk.Presentation/wwwroot/uploads/a97df884-f827-438a-baa1-182827c8ba06.jpg)

![Support Flow](HelpDesk.Presentation/wwwroot/uploads/b6bfd9f2-4ca8-49ff-9d2f-e2c3a89ed094.jpg)

## Prerequisites

Before running the project, make sure you have:

- .NET 8 SDK
- SQL Server instance available
- Visual Studio 2022 or VS Code with C# support

## Configuration

1. Open the file:
   - HelpDesk.Presentation/appsettings.json
2. Update the SQL Server connection string:
   - "HelpDeskConnection"
3. Update the SMTP email settings in the "EmailSettings" section if you want email notifications to work.

Example:

```json
{
  "ConnectionStrings": {
    "HelpDeskConnection": "Server=YOUR_SERVER;Database=HelpDeskDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "EmailSettings": {
    "From": "your-email@example.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@example.com",
    "Password": "your-app-password"
  }
}
```

## Database Setup

The application uses Entity Framework Core with a SQL Server database.

To create/update the database, run:

```bash
dotnet ef database update --project HelpDesk.DAL --startup-project HelpDesk.Presentation
```

If needed, install the EF CLI first:

```bash
dotnet tool install --global dotnet-ef
```

## Run the Application

From the project root, run:

```bash
dotnet restore

dotnet build

dotnet run --project HelpDesk.Presentation
```

Then open the URL shown in the terminal, typically:

- https://localhost:xxxxx
- http://localhost:xxxxx

## Seeded Roles

The application automatically creates these Identity roles when the app starts:

- Admin
- Employee
- Support Engineer

## Notes

- The project is configured for HTTPS redirection and ASP.NET Core Identity authentication.
- Email notifications, ticket activity, and notifications are wired through the service layer and repository layer.
- For a real environment, do not commit sensitive credentials directly into source control.

## License

This project is for educational and internal use unless a separate license is provided by the author.

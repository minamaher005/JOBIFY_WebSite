# Jobify

Jobify is a modern job recruitment and management web application built with ASP.NET Core MVC. It allows job seekers to apply for jobs, manage their profiles, and enables employers to post jobs, manage applications, and company branches.

## Features

- User authentication and role management (Job Seeker, Employee/Employer)
- Job posting, searching, and application workflow
- Company management with support for multiple branches
- Profile editing for job seekers and employees
- Responsive, modern UI with Bootstrap and custom enhancements
- Dynamic forms (add/remove company branches)
- Admin dashboard for managing jobs and applications

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- Bootstrap 5 & Bootstrap Icons
- jQuery (for dynamic form features)
- SQL Server (default, can be changed)

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/minamaher005/JOBIFY_WebSite.git
   cd JOBIFY_WebSite
   ```

2. **Setup the database:**
   - Update `appsettings.json` with your SQL Server connection string.
   - Run migrations:
     ```bash
     dotnet ef database update
     ```

3. **Run the application:**
   ```bash
   dotnet run
   ```
   Or use Visual Studio to build and run.

4. **Access the app:**
   - Open your browser and go to `http://localhost:5089` (or the port shown in your terminal).

## Folder Structure

- `WebApplication2/` - Main ASP.NET Core MVC project
- `WebApplication2/Views/` - Razor views for UI
- `WebApplication2/Controllers/` - MVC controllers
- `WebApplication2/Models/` - Entity models
- `WebApplication2/ViewModels/` - View models for forms
- `WebApplication2/wwwroot/` - Static files (CSS, JS, images)

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.

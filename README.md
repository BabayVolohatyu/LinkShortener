# LinkShortener
# Fullstack .NET 8 + Angular 20 App

This project is a fullstack web application built with **ASP.NET Core 8.0** for the backend and **Angular 20** for the frontend.  
It uses **Entity Framework Core with PostgreSQL** as the database provider.

---

## 📦 Requirements

Make sure you have these installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Node.js (LTS)](https://nodejs.org/en/) (includes npm)  
- [Angular CLI](https://angular.dev/tools/cli)  
- PostgreSQL database  

---

## ⚙️ Backend Setup (.NET)

1. Navigate to the backend folder (where your `.csproj` is):  
   ```bash
   cd LinkShortener
   ```

2. Restore dependencies:  
   ```bash
   dotnet restore
   ```

3. Apply database migrations (make sure PostgreSQL is running and your `appsettings.json` connection string is correct):  
   ```bash
   dotnet ef database update
   ```

4. Run the backend server:  
   ```bash
   dotnet run
   ```

By default, the API will run on: **http://localhost:5091**

---

## 🎨 Frontend Setup (Angular)

1. Navigate to the Angular client folder:  
   ```bash
   cd Client
   ```

2. Install dependencies:  
   ```bash
   npm install
   ```

3. Start the Angular dev server:  
   ```
   npm start
   ```

By default, the Angular app will run on: **http://localhost:4200**

---

## 🚀 Launch Both (Quickstart Script)

For convenience, you can launch both servers in parallel.  

### Linux / macOS
```bash
#!/bin/bash
cd LinkShortener && dotnet ef database update && dotnet run &
cd Client && npm install && npm start
```

### Windows (PowerShell)
```powershell
cd LinkShortener
dotnet ef database update
Start-Process "dotnet" "run"
cd Client
npm install
npm start
```

---

## 🔑 Notes

- Ensure PostgreSQL is running before applying migrations.  
- If you add new EF Core migrations, use:
  ```bash
  dotnet ef migrations add MigrationName
  dotnet ef database update
  ```

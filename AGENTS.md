# AGENTS.md

## Project overview

VB.NET Windows Forms desktop application (`DB_Connect_Sample`) targeting `net10.0-windows`. It connects to a local SQL Server (`TestDB` database) and displays a list of schools (STEP校一覧) in a dropdown. Uses `Microsoft.Data.SqlClient 6.1.4`.

## Cursor Cloud specific instructions

### Build

This is a WinForms project targeting `net10.0-windows`. On Linux you **must** pass `-p:EnableWindowsTargeting=true` to both restore and build:

```bash
dotnet restore DB_Connect_Sample.slnx -p:EnableWindowsTargeting=true
dotnet build DB_Connect_Sample.slnx -p:EnableWindowsTargeting=true
```

The build produces `DB_Connect_Sample/bin/Debug/net10.0-windows/DB_Connect_Sample.dll` but the GUI cannot be launched on Linux (WinForms requires Windows).

### Database (SQL Server)

The app expects SQL Server on `localhost:1433` with a database called `TestDB` containing tables `school` (Id INT, Name NVARCHAR) and `Users` (Id INT, Name NVARCHAR).

Start SQL Server via Docker:

```bash
sudo dockerd &>/tmp/dockerd.log &
sleep 3
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=StrongP@ss123!" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Create the database and tables:

```bash
docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'StrongP@ss123!' -C -Q "
CREATE DATABASE TestDB;
GO
USE TestDB;
CREATE TABLE school (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(100) NOT NULL);
CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(100) NOT NULL);
INSERT INTO school (Name) VALUES (N'STEP東京校'),(N'STEP大阪校'),(N'STEP名古屋校'),(N'STEP福岡校'),(N'STEP札幌校');
INSERT INTO Users (Name) VALUES (N'田中太郎'),(N'鈴木花子'),(N'佐藤一郎'),(N'山田美咲'),(N'高橋健太');
"
```

The source code uses `Integrated Security=True` (Windows Authentication). On Linux with SQL Server in Docker, use SQL Authentication (`User Id=sa;Password=StrongP@ss123!`) instead for testing.

### Limitations on Linux

- **WinForms GUI cannot run on Linux.** The application binary builds successfully but cannot be executed. Testing database logic requires a separate console project or unit tests.
- There are no lint tools, automated tests, or CI/CD pipelines configured in this repository.

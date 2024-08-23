## Dependencies
* .NET 5
* ASP.NET Core 5
* Docker
* SQL Server
* Visual Studio 2022
* Additional dependencies are listed in `WebApplication1.csproj` 

&nbsp;

## Setup
To get started, follow the following steps:
1. Install .NET 5 SDK on your local computer
2. For Mac and Linux users, install Docker. For Windows users, install Docker Desktop on your local computer
3. Open the Command Prompt and execute the following command to download the SQL Server Docker image

   ```bash
   docker pull mcr.microsoft.com/mssql/server
   ```

4. Start the Docker container by running the following command

   Windows

   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YOUR_PASSWORD" -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Mac/Linux

   ```bash
   docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=YOUR_PASSWORD' -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Replace `YOUR_PASSWORD` with the password you would use to connect to SQL Server

5. Clone the repository to the local environment using: `git clone <repository-url>`
6. Open the Command Prompt and run the following command within the project's root directory to restore project dependencies

    ```bash
    dotnet restore
    ```

    Afterwards, run the following command to build the project

    ```bash
    dotnet build
    ```
   
7. Open the project in Visual Studio and navigate to the `appsettings.json` file. In the `ConnectionString` section, replace the `Password` field with your SQL server password. If you follow the steps above, you should also set the `User Id` field to `sa` and choose a database name to enter in the `Database` field
8. Open the Command Prompt and run the following command to create the initial database migration

   ```bash
   dotnet ef migrations add initialMigration
   ```

   Afterwards, execute the following command to create the database

   ```bash
   dotnet ef database update
   ```

9. After completing the steps above, open Visual Studio to run the project. Alternatively, you could use the
    
   ```bash
   dotnet run
   ```
   
   command in the project's root directory
   
    
 




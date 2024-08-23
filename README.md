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
2. For Mac/Linux users, install Docker; For Windows users, install Docker Desktop on local computer
3. Open command prompt and run the following command to get the SQL Server docker image:

   ```bash
   docker pull mcr.microsoft.com/mssql/server
   ```

4. Start the docker container by running the following command:

   Windows:

   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YOUR_PASSWORD" -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Mac/Linux:

   ```bash
   docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=YOUR_PASSWORD' -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Replace `YOUR_PASSWORD` with the password you would use to connect to the SQL Server database

5. Clone the repository to the local environment using: `git clone <repository-url>`
6. Open command prompt and run the following command within the project's root directory to restore project dependencies:

    ```bash
    dotnet restore
    ```

    Afterwards, run the following command to build the project:

    ```bash
    dotnet build
    ```
   
7. Use Visual Studio to open the project and navigate to the `appsettings.json` file. Replace the `Password` field in the `ConnectionString` section according to your SQL server password (If you follow the above steps, you could fill in the `User Id` field as `sa` and pick a database name of your choice to fill in the `Database` field)
8. Open command prompt and run the following command to add initial database migration:

   ```bash
   dotnet ef migrations add initialMigration
   ```

   Afterwards, execute the following command to create the database:

   ```bash
   dotnet ef database update
   ```

9. Once the above step is finished, open Visual Studio to run the project. Alternatively, you could also use
    
   ```bash
   dotnet run
   ```
   
   in the project's root directory to run the project
   
    
 




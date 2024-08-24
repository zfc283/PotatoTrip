## Description

PotatoTrip is an ASP.NET Core backend solution designed to streamline travel deals shopping experiences. This robust project offers a suite of RESTful APIs supporting CRUD operations on travel deals, user registration and authentication, along with mechanisms for handling shopping cart and order management. Ideal for trip-vendor-related applications looking for robust backend support.

&nbsp;

- Project Demo: http://99.79.181.180:5000 &nbsp; (Details on API usage are outlined below)
- Test account:
  - email: admin@&#8203;potatotrip.site
  - password: Abc123$

&nbsp;

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
3. Open the Command Prompt and execute the following command to download the SQL Server Docker image:

   ```bash
   docker pull mcr.microsoft.com/mssql/server
   ```

4. Start the Docker container by running the following command:

   Windows:

   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YOUR_PASSWORD" -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Mac/Linux:

   ```bash
   docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=YOUR_PASSWORD' -p 1433:1433 -d mcr.microsoft.com/mssql/server
   ```

   Replace `YOUR_PASSWORD` with the password you would use to connect to SQL Server

5. Clone the repository to the local environment using: `git clone <repository-url>`
6. Open the Command Prompt and run the following command within the project's root directory to restore project dependencies:

    ```bash
    dotnet restore
    ```

    Afterwards, run the following command to build the project:

    ```bash
    dotnet build
    ```
   
7. Open the project in Visual Studio and navigate to the `appsettings.json` file. In the `ConnectionString` section, replace the `Password` field with your SQL server password. If you follow the steps above, you should also set the `User Id` field to `sa` and choose a database name to enter in the `Database` field
8. Open the Command Prompt and run the following command to create the initial database migration:

   ```bash
   dotnet ef migrations add initialMigration
   ```

   Afterwards, execute the following command to create the database:

   ```bash
   dotnet ef database update
   ```

9. After completing the steps above, open Visual Studio to run the project. Alternatively, you could use the
    
   ```bash
   dotnet run
   ```
   
   command in the project's root directory

&nbsp;

## API Usage
### Root Endpoint
- **Path**: http:&#8203;//99.79.181.180:5000/api
- **Method**: GET
- **Description**: This HATEOAS-driven endpoint provides an overview of available APIs in the application, offering a dynamic entry point for navigating the API landscape.
- **Request Parameters**: None

#

### Travel Routes
- **Path**: http:&#8203;//99.79.181.180:5000/api/travelRoutes
- **Method**: GET
- **Description**: Retrieves all travel deals available in the application. The results can be filtered based on request parameters. By default, the response is in `application/json`. To enable HATEOAS, set the `Accept` header to `application/vnd.mycompany.hateoas+json`.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|------------|--------------------|----------|--------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Accept             | String   | No           | Set to `application/vnd.mycompany.hateoas+json` to enable HATEOAS                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| **Query**  | keyword            | String   | No           | Filter travel routes based on if title of a travel route contains the keyword                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
|            | pageNumber         | String   | No           | Specifies the page number in a paginated response. Must be a positive integer. Default value is 1.                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|            | pageSize           | String   | No           | Specifies the number of items to be returned per page in a paginated response. Must be a positive integer. Default value is 10, maximum value is 50.                                                                                                                                                                                                                                                                                                                                                                                             |
|            | rating             | String   | No           | Filter travel routes based on rating. Use the format: `$operatorType $value`. `$operatorType` is one of `lessThan`, `equalTo`, `largerThan`. `$value` is an integer representing the rating. Example: `rating=largerThan 3`                                                                                                                                                                                                                                                                                                                      |
|            | orderBy            | String   | No           | Sorts the response by specified attributes in ascending or descending order. Specify sorting criteria by using the attribute followed by `asc` (ascending) or `desc` (descending), separated by commas. If the direction is omitted, results are sorted in ascending order by default. Supported attributes are: `id`, `title`, `originalPrice`, `departureTime`, `rating`, `travelDays`. Incorrect or unsupported attributes will trigger an error. Example: `orderBy=title desc, originalPrice`                                                |
|            | fields             | String   | No           | Specifies a subset of fields to be returned for each travel route item. List the desired fields separated by commas. For example, using `fields=id, price` will return only the `id` and `price` fields in each travel route. **Note**: When HATEOAS is enabled, the `id` field **must** be included in the `fields` parameter to avoid issues. Supported fields include `id`, `title`, `description`, `price`, `originalPrice`, `discountPercent`, among others. |


   
    
 




## Description

PotatoTrip is an ASP.NET Core backend solution designed to streamline travel deals shopping experiences. This robust project offers a suite of RESTful APIs supporting CRUD operations on travel deals, user registration and authentication, along with mechanisms for handling shopping cart and order management. Ideal for trip-vendor-related applications looking for robust backend support.

&nbsp;

- Project Demo: https://potatotrip.site/api &nbsp; (Details on API usage are outlined below)
- Admin account:
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
- **Path**: https:&#8203;//potatotrip.site/api
- **Method**: GET
- **Description**: This HATEOAS-driven endpoint provides an overview of available APIs in the application, offering a dynamic entry point for navigating the API landscape.
- **Request Parameters**: None

#

### Travel Routes
- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes
- **Method**: GET
- **Description**: Retrieves all travel deals available in the application. The results can be filtered based on request parameters. By default, the response is in `application/json`. To enable HATEOAS, set the `Accept` header to `application/vnd.mycompany.hateoas+json`.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|------------|--------------------|----------|--------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Accept             | String   | No           | Default is `application/json`. Set to `application/vnd.mycompany.hateoas+json` to enable HATEOAS                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| **Query**  | Keyword            | String   | No           | Filter travel routes based on if title of a travel route contains the keyword                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
|            | PageNumber         | String   | No           | Specifies the page number in a paginated response. Must be a positive integer. Default value is 1.                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|            | PageSize           | String   | No           | Specifies the number of items to be returned per page in a paginated response. Must be a positive integer. Default value is 10, maximum value is 50.                                                                                                                                                                                                                                                                                                                                                                                             |
|            | Rating             | String   | No           | Filter travel routes based on rating. Use the format: `$operatorType $value`. `$operatorType` is one of `lessThan`, `equalTo`, `largerThan`. `$value` is an integer representing the rating. Example: `rating=largerThan 3`                                                                                                                                                                                                                                                                                                                      |
|            | OrderBy            | String   | No           | Sorts the response by specified attributes in ascending or descending order. Specify sorting criteria by using the attribute followed by `asc` (ascending) or `desc` (descending), separated by commas. If the direction is omitted, results are sorted in ascending order by default. Supported attributes are: `id`, `title`, `originalPrice`, `departureTime`, `rating`, `travelDays`. Incorrect or unsupported attributes will trigger an error. Example: `orderBy=title desc, originalPrice`                                                |
|            | Fields             | String   | No           | Specifies a subset of fields to be returned for each travel route item. List the desired fields separated by commas. For example, using `fields=id, price` will return only the `id` and `price` fields in each travel route. **Note**: When HATEOAS is enabled, the `id` field **must** be included in the `fields` parameter to avoid issues. Supported fields include `id`, `title`, `description`, `price`, `originalPrice`, `discountPercent`, among others. |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes
- **Method**: POST
- **Description**: Creates a new travel route in the application. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful creation, the API returns the created travel route in the response body.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name**  | **Type** | **Required** | **Description**                                                                                                                       |
|------------|---------------------|----------|--------------|---------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token.                                     |
| **Body**   | Title               | String   | Yes          | The title field needs to be different from the description field                                                                      |
|            | Description         | String   | Yes          | The description field needs to be different from the title field                                                                      |
|            | OriginalPrice       | Decimal  | No           |                                                                                                                                       |
|            | DiscountPercent     | Decimal  | No           | Decimal value in the range [0.0 to 1.0]                                                                                               |
|            | DepartureTime       | String   | No           | Date format in `yyyy-MM-dd`. Example: `2024-05-01`                                                                                    |
|            | Features            | String   | No           |                                                                                                                                       |
|            | Fees                | String   | No           |                                                                                                                                       |
|            | Notes               | String   | No           |                                                                                                                                       |
|            | TravelRoutePictures | Array    | No           | An array of objects. Each object contains only the field `url`.                                                                       |
|            | Rating              | Decimal  | No           |                                                                                                                                       |
|            | TravelDays          | String   | No           | Specifies the number of travel days. Accepted values are: `One`, `Two`, `Three`, `Four`, `Five`, `Six`, `Seven`, `Eight`, `EightPlus` |
|            | TripType            | String   | No           | Specifies the type of trip. Accepted values are: `HotelAndAttractions`, `Group`, `PrivateGroup`, `BackPackTour`, `SemiBackPackTour`.  |
|            | DepartureCity       | String   | No           | Specifies the trip departure city. Accepted values are: `Beijing`, `Shanghai`, `Guangzhou`, `Shenzhen`.                               |
    
&nbsp;

**Example request body**:

```JSON
{
    "title": "hellotest333",
    "description": "hello test hello test hello test",
    "originalPrice": 6988.9,
    "departureTime": "2024-09-01",
    "travelRoutePictures": [
        {
            "url": "../../assets/images/osaka-castle-1398116_640.jpg"
        },
        {
            "url": "../../assets/images/222222.jpg"
        }
    ],
    "tripType": "group",
    "travelDays": "three",
    "departureCity": "beijing"
}
```

&nbsp;

#

### Authentication
- **Path**: https:&#8203;//potatotrip.site/api/auth/register
- **Method**: POST
- **Description**: User registration. Upon successful registration, the API returns a 200 OK status code.
- **Request Parameters**:

&nbsp;

|          | **Parameter Name** | **Type** | **Required** | **Description**                                                                                                                   |
|----------|--------------------|----------|--------------|-----------------------------------------------------------------------------------------------------------------------------------|
| **Body** | Email              | String   | Yes          | This parameter is used as the login credential and must be a unique email address within the system                               |
|          | Password           | String   | Yes          | The password needs to contain numbers, uppercase and lowercase characters, special characters and must be at least 6 digits long  |
|          | ConfirmPassword    | String   | Yes          | The confirm password needs to be identical to the entered password                                                                |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/auth/login
- **Method**: POST
- **Description**: User Login. This API authenticates the user and, upon successful login, returns a JWT (JSON Web Token) in the response body. This token is used to verify the user's identity in subsequent requests.
- **Request Parameters**:

&nbsp;

|          | **Parameter Name** | **Type** | **Required** | **Description**                              |
|----------|--------------------|----------|--------------|----------------------------------------------|
| **Body** | Email              | String   | Yes          | Email provided at registration   |
|          | Password           | String   | Yes          | Password entered at registration |

&nbsp;

#

### Travel Route
- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}
- **Method**: GET
- **Description**: Gets a single travel route with ID `travelRouteID`
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                                                                                                                                                                                                                                                                                                                                                 |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Accept             | String   | No           | Default is `application/json`. Set to `application/vnd.mycompany.hateoas+json` to enable HATEOAS                                                                                                                                                                                                                                                                                                                                |
| **Query**  | Fields             | String   | No           | Specifies a subset of fields to be returned. List the desired fields separated by commas. For example, using `fields=id, price` will return only the `id` and `price` fields in the result. **Note**: When HATEOAS is enabled, the `id` field **must** be included in the `fields` parameter to avoid issues. Supported fields include `id`, `title`, `description`, `price`, `originalPrice`, `discountPercent`, among others. |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}
- **Method**: PUT
- **Description**: Updates a single travel route with ID `travelRouteID`. Note that this method performs a **complete update**: any fields not specified in the request will be reset to their default values (for example: null), potentially overwriting any existing values. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful update, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;


|            | **Parameter Name**  | **Type** | **Required** | **Description**                                                                                                                       |
|------------|---------------------|----------|--------------|---------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token.                                     |
| **Body**   | Title               | String   | Yes          | The title field needs to be different from the description field                                                                      |
|            | Description         | String   | Yes          | The description field needs to be different from the title field                                                                      |
|            | OriginalPrice       | Decimal  | No           |                                                                                                                                       |
|            | DiscountPercent     | Decimal  | No           | Decimal value in the range [0.0 to 1.0]                                                                                               |
|            | DepartureTime       | String   | No           | Date format in `yyyy-MM-dd`. Example: `2024-05-01`                                                                                    |
|            | Features            | String   | No           |                                                                                                                                       |
|            | Fees                | String   | No           |                                                                                                                                       |
|            | Notes               | String   | No           |                                                                                                                                       |
|            | TravelRoutePictures | Array    | No           | An array of objects. Each object contains only the field `url`.                                                                       |
|            | Rating              | Decimal  | No           |                                                                                                                                       |
|            | TravelDays          | String   | No           | Specifies the number of travel days. Accepted values are: `One`, `Two`, `Three`, `Four`, `Five`, `Six`, `Seven`, `Eight`, `EightPlus` |
|            | TripType            | String   | No           | Specifies the type of trip. Accepted values are: `HotelAndAttractions`, `Group`, `PrivateGroup`, `BackPackTour`, `SemiBackPackTour`.  |
|            | DepartureCity       | String   | No           | Specifies the trip departure city. Accepted values are: `Beijing`, `Shanghai`, `Guangzhou`, `Shenzhen`.                               |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}
- **Method**: PATCH
- **Description**: Partially updates a single travel route with ID `travelRouteID`. This method performs a partial update: any fields not specified in the request will not be reset to their default values, only those fields specified in the request will be updated. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful update, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization     | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |
| **Body**   | N/A                | Array    | Yes          | JSON Patch document describing changes to data fields                                    |

&nbsp;

**Example request body**:

```JSON
[
  { "op": "replace", "path": "/title", "value": "Two day trip to New York" },
  { "op": "replace", "path": "/originalPrice", "value": 2599 },
  { "op": "remove", "path": "/discountPercent" },
  { "op": "replace", "path": "/picture/url", "value": "../images/123456.png" }
]
```

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}
- **Method**: DELETE
- **Description**: Deletes a single travel route with ID `travelRouteID`. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful deletion, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token. |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/(travelRouteID1, travelRouteID2, ...)
- **Method**: DELETE
- **Description**: Batch deletes travel routes based on the travel route IDs enclosed within the parentheses in the path. Example call of API: `http://99.79.181.180:5000/api/travelRoutes/(1, 2, 3, 4)` (Travel routes with ID 1, 2, 3, 4 will be deleted). Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful deletion, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token. |

&nbsp;

#

### Travel Route Picture
- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}/pictures
- **Method**: GET
- **Description**: Gets all the pictures of the travel route with ID `travelRouteID`
- **Request Parameters**: None

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}/pictures
- **Method**: POST
- **Description**: Adds a picture to the travel route with ID `travelRouteID`. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful creation, the API returns the created travel route picture in the response body.
- **Request Parameters**: 

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                  |
|------------|--------------------|----------|--------------|--------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |
| **Body**   | Url                | String   | Yes          | Url of the picture                                                                               |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}/pictures/{pictureID}
- **Method**: GET
- **Description**: Gets the picture with ID `pictureID` under the travel route with ID `travelRouteID`
- **Request Parameters**: None

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/travelRoutes/{travelRouteID}/pictures/{pictureID}
- **Method**: DELETE
- **Description**: Deletes the picture with ID `pictureID` under the travel route with ID `travelRouteID`. Authentication and admin rights are required (to access admin rights, log in with the admin account). Upon successful deletion, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

#

### Shopping Cart
- **Path**: https:&#8203;//potatotrip.site/api/shoppingCart
- **Method**: GET
- **Description**: Gets the shopping cart for the current logged in user.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/shoppingCart/items
- **Method**: POST
- **Description**: Adds a travel deal to the current logged in user's shopping cart
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                  |
|------------|--------------------|----------|--------------|--------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |
| **Body**   | TravelRouteID      | String   | Yes          | ID of the travel route to be added to the shopping cart                                          |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/shoppingCart/items/{itemID}
- **Method**: DELETE
- **Description**: Deletes the item with ID `itemID` from the current logged in user's shopping cart. Upon successful deletion, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/shoppingCart/items/(itemID1, itemID2, ...)
- **Method**: DELETE
- **Description**: Batch deletes shopping cart items based on the item IDs enclosed within the parentheses in the path. Example call of API: `http://99.79.181.180:5000/api/shoppingCart/items/(1, 2, 3, 4)` (Items with ID 1, 2, 3, 4 will be deleted). Log in is required. Upon successful deletion, the API returns a 204 No Content status code.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/shoppingCart/checkout
- **Method**: POST
- **Description**: Checks out the current logged in user's shopping cart and create an order. Upon successful checkout, the API returns details of the created order in the response body.
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

**Example successful response**:

```JSON
{
  "id": "a681fb73-21a8-4ed4-b44c-6adf68c11401",
  "userId": "699d065f-f0de-4624-a4fb-0051e2b2a407",
  "orderItems": [
    {
      "id": 1,
      "travelRouteId": "39996f34-013c-4fc6-b1b3-0c1036c47110",
      "travelRoute": {
        "id": "39996f34-013c-4fc6-b1b3-0c1036c47110",
        "title": "12-Day Group Tour of Morocco: Sahara Desert + Casablanca + Marrakech + Chefchaouen (4-star)",
        "description": "[World Encouragement] Celebrity All-Inclusive Package | No Visa Required | Maximum 25 People | Unique Desert Hotel with Stargazing + Casa Grande 5-Star Sheraton | Includes Camel Ride with Costume Show + Sahara 4x4 Adventure + YSL Garden Afternoon Tea + Performance Show | Trendy Restaurant",
        "price": 15490.00,
        "originalPrice": 15490.00,
        "discountPercent": null,
        "createTime": "0001-01-01T00:00:00",
        "updateTime": null,
        "departureTime": null,
        "features": null,
        "fees": null,
        "notes": null,
        "rating": 3.2,
        "travelDays": "Three",
        "tripType": "BackPackTour",
        "departureCity": "Shenzhen",
        "travelRoutePictures": []
      },
      "shoppingCartId": null,
      "orderId": "a681fb73-21a8-4ed4-b44c-6adf68c11401",
      "originalPrice": 15490.00,
      "discountPercent": null
    }
  ],
  "state": "Pending",
  "createTime": "2024-08-25T02:29:15.8603283+00:00",
  "transactionMetadata": null
}
```

&nbsp;

#

### Order
- **Path**: https:&#8203;//potatotrip.site/api/orders
- **Method**: GET
- **Description**: Gets all orders of the current logged in user
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                                                                      |
|------------|--------------------|----------|--------------|------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token.                                                    |
| **Query**  | PageNumber         | String   | No           | Specifies the page number in a paginated response. Must be a positive integer. Default value is 1.                                                   |
|            | PageSize           | String   | No           | Specifies the number of items to be returned per page in a paginated response. Must be a positive integer. Default value is 10, maximum value is 50. |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/orders/{orderID}
- **Method**: GET
- **Description**: Gets the order with ID `orderID` of the current logged in user
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;

- **Path**: https:&#8203;//potatotrip.site/api/orders/{orderID}/placeOrder (Not yet implemented)
- **Method**: POST
- **Description**: Places the order with ID `orderID` for the current logged in user
- **Request Parameters**:

&nbsp;

|            | **Parameter Name** | **Type** | **Required** | **Description**                                                                                   |
|------------|--------------------|----------|--------------|---------------------------------------------------------------------------------------------------|
| **Header** | Authorization      | String   | Yes          | `bearer [JWT token]`. See the 'Authentication' section for instructions on getting the JWT token |

&nbsp;


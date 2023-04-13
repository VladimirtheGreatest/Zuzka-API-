#Document Storage and Retrieval API 🦜
This repository contains ASP.NET Core service that provides an API for storing and retrieving documents in various formats. The API supports POST and PUT requests for modifying documents, and GET requests for retrieving documents in different formats such as XML, JSON. It is easy to add support for new formats and underlying storage providers, like Redis, InMemory, etc.

##Installation and Usage
To use this API, you must first clone or download this repository. The solution is available inside the project Zuzka.Api.

After downloading, open the solution in Visual Studio and set the startup project to Zuzka.Api. You will need to ensure that .NET6 and SQL Server Express LocalDB are installed on your machine. If you do not have these installed, you can download it here https://dotnet.microsoft.com/en-us/download/dotnet/6.0, https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16

Next, open the Package Manager Console in Visual Studio and run the command 'update-database' to create the local database.

You can now build the project and run it locally. The API is configured to use in-memory caching to speed up reading queries. However, this can easily be replaced by a different provider, such as Redis.

The API schema and documentation are available through Swagger. After starting the application, it will navigate to url/swagger to access the Swagger UI. From here, you can explore the available endpoints and interact with the API directly, otherwise you can use tools such as Postman.

##Unit Tests
This API includes a comprehensive set of unit tests to ensure that the code is functioning correctly. To run the unit tests, simply build the solution and run the tests from within Visual Studio.

This app has been designed using a repository pattern, with separation of concerns between the application's layers. Zuzka.Data responsible for the data access while Zuzka.services store the business logic.

##
Building this app was a lot of fun! Please note that this project is not production-ready, as the database is currently very small and local(probably size of the potato) and many configurations are hardcoded. However, the architecture has been designed with scalability in mind, and it can be easily modified to accommodate larger datasets and different data sources(such as sql server and app configuration)

If you have any questions or need help setting up the project on your local machine, don't hesitate to reach out. I'm happy to assist in any way I can. Thank you for checking out my project!

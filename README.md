Project Overview

Sensor APIs
The Sensor APIs project is a web-based application designed to manage sensor data efficiently.
The application is built using ASP.NETCore, integrating various modern technologies and frameworks to ensure robust functionality, 
scalability, and maintainability. The project encompasses user authentication, sensor data management, 
and versioned APIs to cater to different versions of client applications.

Key Technologies and Frameworks
1. ASP.NETCore
Description: ASP.NETCore is a cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications.

Usage: Utilized for creating the API endpoints, handling HTTP requests, and structuring the overall application architecture.

2. Entity Framework Core (EF Core)
Description: EF Core is an open-source, lightweight, extensible, and cross-platform version of Entity Framework,
a popular Object-Relational Mapping (ORM) framework.

Usage: Employed for database operations, managing data access, and ensuring seamless interactions with the PostgreSQL database.

3. PostgreSQL
Description: PostgreSQL is a powerful, open-source object-relational database system.

Usage: Used as the primary database for storing sensor data and user information.

4. Swagger / Swashbuckle
Description: Swagger is a framework for API documentation, and Swashbuckle is an implementation for ASP.NETCore.

Usage: Used to generate interactive API documentation, making it easier for developers to explore and test the APIs.

5. Microsoft Identity
Description: Microsoft Identity is a robust framework for managing user identities and roles.

Usage: Implemented for user registration, login, and role management within the application.

6. ApiVersioning
Description: A library that provides API versioning capabilities for ASP.NETCore applications.

Usage: Ensures that different versions of the API can coexist, allowing for gradual upgrades and backward compatibility.

7. Logging (Microsoft.Extensions.Logging)
Description: A flexible and extensible framework for logging within ASP.NETCore applications.

Usage: Used to log information, warnings, and errors to assist in debugging and maintaining the application.

8. Memory Caching (IMemoryCache)
Description: Provides a simple in-memory caching mechanism.

Usage: Used to temporarily store sensor data for quick retrieval and reduce database load.

9. Moq
Description: Moq is a popular .NET library for creating mock objects in unit tests.

Usage: Used in unit tests to mock dependencies and isolate the code being tested.

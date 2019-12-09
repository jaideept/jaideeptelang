# Contact demo CRUD application implemented with the Angular CLI and an endpoint using ASP.NET Core Web API using Dapper ORM.

The project has the following features:

* Keep the Angular project code completely separate from the ASP.NET Core code to make updates of either technology easier in the future. This was a key consideration when organizing the folders/files in the project.

* Dapper ORM has been used in .Net Core Api. Using Dapper, it is very easy to fire a SQL query against database and get the result mapped to C# domain class. Dapper makes use of parameterized queries which help protect against SQL injections.

* automatic model validation and 400 response. With ASP.NET Core 2.1, ASP.NET Core 2.1 introduces a new [ApiController] attribute that will trigger automatic model validation and 400 response.Consequently, the following code is unnecessary in an action method:

```
if (!ModelState.IsValid)
{
    return BadRequest(ModelState);
}
```

* Support running the Angular project completely separate from the ASP.NET Core Web API if desired (CORS is enabled in the Startup.cs project). See the notes below if you want to use this option.

* Swagger has been implemented in the Startup.cs to provide a clean view of the REST API to provide a better API documentation to the consumer of the REST API. 

* various security HTTP response headers (X-Xss-Protection, X-Frame-Options, Content-Security-Policy) have been implemented in the Startup.cs to handle few of the OWASP top 10 Vulnerabilities.Dirty, malformed text strings should be used as user-input data to carry out thourough web penetration testing. Source - (https://github.com/minimaxir/big-list-of-naughty-strings). https://securityheaders.com/ can be used to validate the headers in the application hosted over public IP. 

* Data access specific logic is encapsulated in the Repository layer of the app. For example, 'ContactRepository' implements an abstract class 'SqlRepository' Other data source for use in near future could be Document DB and XML repositories.

* CORS is enabled in the 'Startup.cs' file.

## Running the Project

To run the project perform the following steps:

1. Install Node.js 10.16.x or higher - https://nodejs.org

2. Install ASP.NET core 3.0 or higher - https://dot.net

3. Download and configure SQL Server 2017 Express from https://www.microsoft.com/en-in/sql-server/sql-server-editions-express and run Contact table creation script from DataAccess\SQL scripts  

4. Install the Angular CLI:

    'npm install -g @angular/cli'
    Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the   
    source files.
    Run 'ng generate component component-name' to generate a new component. You can also use 'ng generate     
    directive|pipe|service|class|gaurd|interface|enum|module'.

5. Open a command prompt and 'cd' into the project's 'ContactWebAppUI' folder

6. Run 'npm install'

7. Run ng build --prod="true" or ng build --prod. The build command also creates a new folder called dist for distribution. These are the files we can host on a server and our Angular app will load up. (dist/ContactWebAppUI)

8. Open a new command window in the root of the project and run the following commands:

```
dotnet restore
dotnet build -c Debug or dotnet build -c Release
dotnet watch -p ContactApi/ContactApi.csproj  run
```

9. Visit http://localhost:5000/swagger/index.html in the browser to see swagger documentation. http://localhost:5000/contact would invoke GetAll end point.

10. visit `http://localhost:4200/`. it would open up the Angular CRUD screen.

11. a postman collection "ContactApi.postman_collection" is available inside ContactApi project to facilitate web api end point testing using Postman. this collection can be further extended with various combintions of input data for POST / PUT methods.

A few additional notes:

Due to time constraints, following are the items which i wish could have been implemented:

* Running Web Api unit testing via XUnit.
* Running unit tests via [Karma](https://karma-runner.github.io).
* Running end-to-end tests via [Protractor](http://www.protractortest.org/)
* SQL injections testing using sqlmap (http://sqlmap.org/)
* Test this app using list of naughty strings (https://github.com/minimaxir/big-list-of-naughty-strings)

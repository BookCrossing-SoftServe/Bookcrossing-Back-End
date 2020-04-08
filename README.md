# BookCrossingBackEnd  
Platform for book crossing between company employees
[Website](https://localhost:44370/)  
  
## Git Flow  
We are using simpliest github flow to organize our work:  
![Github flow](https://scilifelab.github.io/software-development/img/github-flow.png)  
We have **master** , **develop** and **feature** branches.   
All features must be merged into develop branch!!!
Only the release should merge into the main branch!!!

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Prerequisites
[Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 

### Installing
2. In project BookCrossingBackEnd create file appsettings.json. And paste the code below.
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(LocalDB)\\MSSQLLocalDB;Database=BookCrossingDB;Trusted_Connection=True;MultipleActiveResultSets=true",
    "AzureConnection": "Server=tcp:{server_name}.database.windows.net,1433;Initial Catalog={database_name};Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"
  },
  "MongoSettings": {
    "ConnectionString": "mongodb://{Server name}",
    "DatabaseName": "BookCrossingDB"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Data": {
    "DefaultConnection": {
      "ConnectionString": ""
    }
  },
  "Jwt": {
    "Key": "SoftServeBookCrossingSecretKey",
    "Issuer": "BookCrossing.com"
  }
}
```

1. Clone it from git hub with $ git clone https://github.com/Lv-492-SoftServe/Bookcrossing-Back-End.git 
3. Run BookCrossingBackEnd project
  
**Note! Contribution rules:**  
1. All Pull Requests should start from prefix *#xxx-yyy* where *xxx* - task number and and *yyy* - short description 
e.g. #020-CreateAdminPanel  
2. Pull request should not contain any files that is not required by task.  
In case of any violations, pull request will be rejected.

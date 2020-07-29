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
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes using docker containers. 

### Prerequisites
[Docker](https://www.docker.com) version 17.05 or higher
[Docker-compose](https://github.com/docker/compose)


### Installing
1. Clone repository from GitHub with $ git clone https://github.com/Lv-492-SoftServe/Bookcrossing-Back-End.git 

2. Move to the Bookcrossing-Back-End/src/BookCrossingBackEnd/ and create file appsettings.json then paste the code below.
```
{
  "iKeyForDevelop": "1efe21aa-574a-49cc-ab53-8e93c75074bf",

  "iKeyForProduction": "1f191e43-3248-4c80-9160-d12ba9f10044",

  "EmailConfiguration": {
    "From": "{sender_email}",
    "SmtpServer": "{smtp_server}",
    "Port": 587,
    "Username": "{username}",
    "Password": "{Password}"
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
  },

  "StorageConfiguration": {
    "FolderForBookImages": "book_images"
  }
}
```
###### 07.29.2020

3. Move back to Bookcrossing-Back-End and execute "docker-compose up"
  
**Note! Contribution rules:**  
1. All Pull Requests should start from prefix *#xxx-yyy* where *xxx* - task number and and *yyy* - short description 
e.g. #020-CreateAdminPanel  
2. Pull request should not contain any files that is not required by task.  
In case of any violations, pull request will be rejected.

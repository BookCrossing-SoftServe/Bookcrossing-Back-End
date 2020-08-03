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

###### Note: It's better to use [docker-desktop](https://www.docker.com/products/docker-desktop) if you are on windows

### Installing
1. Clone repository from GitHub with $ git clone https://github.com/Lv-492-SoftServe/Bookcrossing-Back-End.git 

2. Move to Bookcrossing-Back-End and execute "docker-compose up"

###### Note: Also check Bookcrossing-Back-End/src/BookCrossingBackEnd/examplesettings.json and save it as appsettings.json if you want to deploy without docker-compose or pass any extra variables!
  
**Note! Contribution rules:**  
1. All Pull Requests should start from prefix *#xxx-yyy* where *xxx* - task number and and *yyy* - short description 
e.g. #020-CreateAdminPanel  
2. Pull request should not contain any files that is not required by task.  
In case of any violations, pull request will be rejected.

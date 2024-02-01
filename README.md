# BS23-SC24-Assignment-Project
## This project consists of 2 parts
1. BS23-SC24-Assignment-Backend (With BS23-SC24-Assignment-Backend.test for unit testing)
2. BS23-SC24-Assignment-Frontend

## Dependencies:
1. .Net (8.0 LTS)
2. Node (20.11.0 LTS)

## Get the project:
1. Clone the repository with the command: `git clone https://github.com/aburifat/BS23-SC24-Assignment-Project.git`

![](/Screenshots/clone-project.jpg)

## Run the backend of the project:
1. Navigate the folder: `BS23-SC24-Assignment-Project/BS23-SC24-Assignment-Backend`
2. Open the solution file with Visual Studio: `BS23-SC24-Assignment-Backend.sln`
3. Configure ConnectionStrings in `appsettings.json` to connect with your PostgreSQL database or you can change the database in `Program.cs` by commenting out the AddDbContext of PostgreSQL and uncommenting the AddDbContext of SQLite. Currently, this project supports any of the PostgreSQL and SQLite databases.
4. Open NuGet Package Manager Console from: `Tools > NuGet Package Manager > Package Manager Console`.
5. Run this command to create a migration: `Add-Migration init`.
6. Run this command to create the database for the first time: `Update-Database`.
7. Run the project with the `IIS Express` option.

### appsettings.cs Configurations
![](/Screenshots/appsettings-config.jpg)

![](/Screenshots/toggle-database-in-program-cs.jpg)

![](/Screenshots/package-manager-console.jpg)

![](/Screenshots/select-iis-express.jpg)

## Run the frontend of the project:
1. Navigate the folder: `BS23-SC24-Assignment-Project/BS23-SC24-Assignment-Backend`
2. Open the folder with Visual Studio Code.
3. Open the integrated terminal of the Visual Studio Code from: `Terminal > New Terminal`.
4. Run this command to install dependencies: `npm i`.
5. Run this command to start the project `npm run dev`.

## Run the Unit Test:
1. Open the backend project in Visual Studio
2. Right-click on the Project `BS23_SC24_Assignment_Backend.tests` in the Solution Explorer, then select `Run Tests`.
3. Alternatively, run the test from: `Test > Run All Tests`.

![](/Screenshots/run-unit-test.jpg)

## API Requests and Responses
The images below show some API calls using Postman

![](/Screenshots/api-register.jpg)

![](/Screenshots/api-login.jpg)

![](/Screenshots/jwt-token-in-header.jpg)

![](/Screenshots/api-get-task-list.jpg)

![](/Screenshots/api-get-task-by-id.jpg)

![](/Screenshots/api-create-task.jpg)

![](/Screenshots/api-update-task.jpg)

![](/Screenshots/api-delete-task.jpg)

## Frontend Views of the Project

![](/Screenshots/frontend-register.jpg)

![](/Screenshots/frontend-login.jpg)

![](/Screenshots/frontend-my-task-list.jpg)

![](/Screenshots/frontend-admin-all-task-list.jpg)

![](/Screenshots/frontend-create-task.jpg)

![](/Screenshots/frontend-update-task.jpg)

![](/Screenshots/frontend-delete-task.jpg)

![](/Screenshots/frontend-logout.jpg)

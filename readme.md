## Install dotnet 2.x
https://www.microsoft.com/net/learn/get-started/windows

## Adding migrations
To create the migrations when changing ContentDbContext:
Example: dotnet ef migrations add nameofmigration --context Api.Databases.Content.ContentDbContext --project Api/Api.csproj --startup-project Api

`dotnet ef migrations add AutoMigration --context Api.Databases.Content.ContentDbContext -o Databases/Content/Migrations`

To create migrations when changing SchoolsDbContext:

`dotnet ef migrations add AutoMigration --context Api.Databases.Schools.SchoolsDbContext -o Databases/Schools/Migrations`

To create migrations when changing ApiIdentityDbContext:

`dotnet ef migrations add AutoMigration --context Api.Databases.Identity.ApiIdentityDbContext -o Databases/Identity/Migrations`

## Applying Migrations
Execute migrations ContentDbContext:
Example: dotnet ef migrations update --context Api.Databases.Content.ContentDbContext --project Api/Api.csproj --startup-project Api

`dotnet ef database update --context Api.Databases.Content.ContentDbContext`

Execute migrations SchoolsDbContext:

`dotnet ef database update --context Api.Databases.Schools.SchoolsDbContext`

Execute migrations ApiIdentityDbContext:

`dotnet ef database update --context Api.Databases.Identity.ApiIdentityDbContext`




## Run application
Press F5

## Manual publish
`dotnet publish -c Feature | Release | Staging`

## Dev Environment
https://dev-tekman-primaria.azurewebsites.net/backoffice

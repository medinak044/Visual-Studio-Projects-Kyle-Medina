-Microsoft.EntityFrameworkCore
-Microsoft.EntityFrameworkCore.SqlServer // Interacting with MS SQL Server
-Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation // For hot reloading when files are edited

Create Data Models
Add Models to a data context file in a folder called "Data"
Add database connection string to appsettings.json


-Microsoft.EntityFrameworkCore.Tools // For creating Migrations and making changes to database
add-migration AddCategoryToDb
update-database
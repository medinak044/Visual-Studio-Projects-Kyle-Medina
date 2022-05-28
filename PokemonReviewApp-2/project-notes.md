Packages:


----------
Package Manager Console commands:

PM> Add-Migration InitialCreate 
- Adds a named migration file

PM> Update-Database
- Configures database based on migration file(s)


Steps:
- Get packages
- Set up data structures
- Link up project to SQL database via connection string (remember to update appsettings.json with the connection string)
- You can prefill the database with a Seed file

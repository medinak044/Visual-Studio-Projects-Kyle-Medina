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


Workflow to setting up routes/endpoints:
- Assuming the Models, AutoMapperProfiles have been established...
- Create Interface
- Create Repository, implement interface. The Repository file will hold db query logic.
- Create Dto (based on the Model), keeping in mind what data will be sent back to the user.
- Create Controller to handle API endpoint logic. Make sure to include Controller in AutoMapperProfiles,
and add Controller to Program so the API knows to use the Repositories


* "Repository" is where you put your database calls
* Paste into your Program file to prevent Json cycling: (ref: https://youtu.be/FEanWuYq7us?list=PL82C6-O4XrHdiS10BLh23x71ve9mQCln0&t=2891)
builder.Services.AddControllers().AddJsonOptions(x => 
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
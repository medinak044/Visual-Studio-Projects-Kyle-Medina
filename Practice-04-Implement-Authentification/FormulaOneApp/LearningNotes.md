- Remember to exclude "appsettings.json" from application in Git repo and when deployed

- Include a new field in "appsettings.json":
"JwtConfig": {
    "Secret":  "Secret key string"
  }
- Create "JwtConfig.cs" in Configurations folder
- Inject Jwt authorization and Identity in Program.cs
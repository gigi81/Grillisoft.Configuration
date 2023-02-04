[![Build Status](https://luigigrilli.visualstudio.com/Grillisoft.Configuration/_apis/build/status/gigi81.Grillisoft.Configuration?branchName=stable)](https://luigigrilli.visualstudio.com/Grillisoft.Configuration/_build/latest?definitionId=14&branchName=stable)
![Nuget](https://img.shields.io/nuget/v/Grillisoft.Configuration.Abstractions)


# Grillisoft.Configuration

A collection of packages to add 2 features to the Microsoft.Extensions.Configuration configuration system for dotnet:
- FileTree
- Parsing

## File Tree
Create a settings tree where child can override settings of their parent. This is very useful for example to create environments settings where for example UAT is the root and then you have production, qa and development that override the settings of their parent(s). You can use XML or JSON files depending on your taste.

## Parsing
Parsing allows to compose strings depending on other settings keys. It can be used with any configuration source, with, without or in combination with a File Tree

## Install Packages
```bash
dotnet add package Grillisoft.Configuration.Xml
dotnet add package Grillisoft.Configuration.Json
dotnet add package Grillisoft.Configuration.RegExParser
```

## Example usage JSON
The combination of these two features is very powerfull and, for example, it can greatly simplify managament of environment settings.

On uat.json settings file you have these key/value pairs:
```json
{
  "env": "uat",
  "sqlserverhost": "sql-${env}.contoso.com",
  "mailhost": "mail-${env}.contoso.com"
}
```

On prod.json file:
```json
{
  "parent": "uat",
  "env": "prod"
}
```

On dev.json file:
```json
{
  "parent": "uat",
  "env": "dev",
  "sqlserverhost": "localhost"
}
```

Now when requesting the setting for "sqlserverhost", here is the outcome for the above example depending on the environment:
- uat: sql-uat.contoso.com
- prod: sql-prod.contoso.com
- dev: localhost

```csharp
//install nuget package Grillisoft.Configuration.Json
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonTree(new[] { Environment.MachineName, Environment.GetEnvironmentVariable("ENVIRONMENT") })
    .AddRegExParser()
    .Build();
    
Console.WriteLine(configuration["sqlserverhost"]);
```

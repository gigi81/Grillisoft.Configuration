# Grillisoft.Configuration

A collection of packages to add 2 features to the Microsoft.Extensions.Configuration configuration system for dotnet:
- FileTree
- Parsing

## File Tree
Create a settings tree where child can overwrite settings of their parent. This is very useful to create environments settings where for example UAT is the root and then you have production and development that overwrite the settings of the parent. You can use XML or JSON files depending on your taste.

## Parsing
Parsing is very usefull to recycle settings keys and automatically compose strings depending on other settings keys

## Example usage
The combination of these two features is very powerfull and allows extreme flexibility.
For example:
On your UAT settings file you have these key/value pairs:
- env: uat
- sqlserverhost: sql-${env}.contoso.com
- mailhost: mail-${env}.contoso.com

On your PROD file:
- env: prod

On your DEV file:
- env: dev
- sqlserverhost: localhost

Now when requesting the setting for "sqlserverhost", here is the outcome for the above example depending on the environment:
- UAT: sql-uat.contoso.com
- PROD: sql-prod.contoso.com
- DEV: localhost

```csharp
using Grillisoft.Configuration

var configuration = new ConfigurationBuilder()
    AddJsonTree(new[] { Environment.MachineName, Environment.GetEnvironmentVariable("ENVIRONMENT") })
    .AddRegExParser()
    .Build();
    
Console.WriteLine(configuration["sqlserverhost"]);
```

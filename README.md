# Grillisoft.Configuration

A collection of packages to add 2 features to the Microsoft.Extensions.Configuration configuration system for dotnet:
- FileTree
- Parsing

## File Tree
Create a settings tree where child can override settings of their parent. This is very useful for example to create environments settings where for example UAT is the root and then you have production, qa and development that override the settings of their parent(s). You can use XML or JSON files depending on your taste.

## Parsing
Parsing allows to compose strings depending on other settings keys. It can be used with any configuration source, with, without or in combination with a File Tree

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
using Grillisoft.Configuration

var configuration = new ConfigurationBuilder()
    .AddJsonTree(new[] { Environment.MachineName, Environment.GetEnvironmentVariable("ENVIRONMENT") })
    .AddRegExParser()
    .Build();
    
Console.WriteLine(configuration["sqlserverhost"]);
```

## Example usage DOLT

```bash
dolt sql
```

```sql
CREATE TABLE uat (`key` VARCHAR(128), `value` VARCHAR(1024), PRIMARY KEY(`key`));
INSERT INTO uat VALUES('env', 'uat');
INSERT INTO uat VALUES('sqlserverhost', 'sql-${env}.contoso.com');
INSERT INTO uat VALUES('mailhost', 'mail-${env}.contoso.com');

CREATE TABLE prod (`key` VARCHAR(128), `value` VARCHAR(1024), PRIMARY KEY(`key`));
INSERT INTO prod VALUES('.parent', 'uat');
INSERT INTO prod VALUES('env', 'prod');

CREATE TABLE dev (`key` VARCHAR(128), `value` VARCHAR(1024), PRIMARY KEY(`key`));
INSERT INTO dev VALUES('.parent', 'uat');
INSERT INTO dev VALUES('env', 'dev');
INSERT INTO dev VALUES('sqlserverhost', 'localhost');
```

```bash
dolt add uat
dolt add prod
dolt add dev
dolt commit -m "Initial commit"
dolt push --set-upstream origin master
```
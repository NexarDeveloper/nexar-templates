# Nexar.Templates

[nexar.com]: https://nexar.com/
[StrawberryShake]: https://github.com/ChilliCream/hotchocolate

Nexar client dotnet templates.

[nuget.org/packages/Nexar.Templates](https://www.nuget.org/packages/Nexar.Templates/)

## Install

In order to install the templates, invoke:

```
dotnet new -i Nexar.Templates
```

The above command installs the following templates:

### nexar-console-design

C# console app running the query `desWorkspaces`.
For getting the token, it requires environment variables `NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.
You need an application at [nexar.com] with the Design scope.
Use this application client ID and secret for the variables.
In addition, the app starts the system browser for signing in.
You need your Altium Live credentials and have to be a member of at least one Altium 365 workspace.

    dotnet new nexar-console-design

### nexar-console-supply

C# console app running the loop of queries `supSearchMpn`.
For getting the token, it requires environment variables `NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.
You need an application at [nexar.com] with the Supply scope.
Use this application client ID and secret for the variables.

    dotnet new nexar-console-supply

### nexar-console-net4x

This template is for .NET Framework. Note that net472+ is recommended and net461 is the minimum possible.
Unlike other templates, it uses the environment variable `NEXAR_TOKEN`.
You may generate this token at your [nexar.com] application details.

Projects have different target frameworks because Strawberry Shake generated code is netstandard only:
`Nexar.Client` is netstandard2.0 and the main `Program` is net472.

    dotnet new nexar-console-net4x

## Uninstall

In order to uninstall the templates, invoke:

```
dotnet new -u Nexar.Templates
```

## Example

Having installed Nexar templates and ID and secret variables, try the following steps:

```
mkdir MyNexarDesign
cd MyNexarDesign
dotnet new nexar-console-design

cd Program
dotnet run
```

**Explanation:**

`dotnet new` creates the new C# project `MyNexarDesign.csproj` and its source files from the template `nexar-console-design`.
The subdirectory `Nexar.Client` contains the configuration, schema, and the directory `Resources` with app queries.
The file `Queries.graphql` contains a sample query.

`dotnet run` builds and runs the created project:

- The [StrawberryShake] processes queries in `Resources` and generates C# client code in `Generated`.
- `Program.cs` is compiled with generated strongly typed query execution and results.
- When the app runs:
    - it starts the browser for signing in and getting the token
    - it creates and configures the Nexar client with the token
    - it runs the query and prints the results

## Further development

### Update query

Modify or replace the sample query in `Nexar.Client/Resources`.
Build the solution. StrawberryShake regenerates the query and result types.
At this point `Program.cs` using the old query and result may not compile.
Update its code for the new query and result types.
Build and run the updated program.

### Update schema

Nexar GraphQL API is being actively developed and its GraphQL schema is likely to change.
Use the StrawberryShake tools for updating the schema file `Nexar.Client/schema.graphql`.

You may install the tools globally:

    dotnet tool install StrawberryShake.Tools -g

Update the schema with global tools:

    cd Nexar.Client
    dotnet-graphql update

Alternatively, install the tools locally (suitable for source control):

    cd <repository-root>
    dotnet new tool-manifest
    dotnet tool install StrawberryShake.Tools

Then on another machine you may restore local tools as:

    dotnet tool restore

Update the schema with local tools:

    cd Nexar.Client
    dotnet graphql update

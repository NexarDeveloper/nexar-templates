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
dotnet run
```

**Explanation:**

`mkdir` and `cd` create the new directory `MyNexarDesign` and change to it.

`dotnet new` creates the new C# project `MyNexarDesign.csproj` and its source files from the template `nexar-console-design`.
The subdirectory `GraphQL` contains the configuration, schema, and the directory `Resources` with app queries.
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

Modify or replace the sample query in GraphQL folder with your custom query.
Build the project. StrawberryShake regenerates the query and result types.
At this point `Program.cs` using the old query and results may not compile.
Update its code to call the new query and process the new results.
Build and run the updated program.

### Update schema

Nexar GraphQL API is being actively developed and its GraphQL schema is likely to change.
Use the StrawberryShake tools for updating the schema file `GraphQL/schema.graphql`.

You may install the tools globally:

    dotnet tool install StrawberryShake.Tools -g

Update the schema with global tools:

    cd <project-path>/GraphQL
    dotnet-graphql update

Alternatively, install the tools locally (suitable for source control):

    cd <repository-root>
    dotnet new tool-manifest
    dotnet tool install StrawberryShake.Tools

Then on another machine you may restore local tools as:

    dotnet tool restore

Update the schema with local tools:

    cd <project-path>/GraphQL
    dotnet graphql update

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

C# .NET 6 console app running the query `desWorkspaces`.
For getting the token, it requires environment variables `NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.
You need an application at [nexar.com] with the Design scope.
Use this application client ID and secret for the variables.
In addition, the app starts the system browser for signing in.
You need your Altium Live credentials and have to be a member of at least one Altium 365 workspace.

    dotnet new nexar-console-design

### nexar-console-supply

C# .NET 6 console app running the loop of queries `supSearchMpn`.
For getting the token, it requires environment variables `NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.
You need an application at [nexar.com] with the Supply scope.
Use this application client ID and secret for the variables.

This sample also shows how to automatically update the token.

    dotnet new nexar-console-supply

### nexar-console-net4x

C# .NET Framework console app. Note that net472+ is recommended and net461 is the minimum possible.
Unlike other templates, the app uses the environment variable `NEXAR_TOKEN`, to show this simpler scenario.
You may generate this token at your [nexar.com] application details. Copy and use the token until it expires.

Projects have different target frameworks because Strawberry Shake generated code is netstandard only.
`Nexar.Client` is netstandard2.0 (StrawberryShake client) and `Program` is net472 (the main app).

    dotnet new nexar-console-net4x

## Uninstall

In order to uninstall the templates, invoke:

```
dotnet new -u Nexar.Templates
```

## Example

Having installed Nexar templates and the client ID and secret variables, try the following steps:

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

### Compose GraphQL

Modify or replace the sample query in `Nexar.Client/Resources`.
Build the solution. StrawberryShake regenerates the query and result types.
At this point `Program.cs` using the old query and result may not compile.
Update its code for the new query and result types.
Build and run the updated program.

You can add new GraphQL operations (queries, mutations) to the same file or
create additional `*.graphql` files. Note that anonymous operations are not
supported, names are used for the corresponding generated C# types names.

### GraphQL IDEs

Composing queries in `*.graphql` files using plain text editors is possible but tedious and error prone.
Instead or in addition, consider using one of GraphQL IDEs, for example one of these:

#### VSCode GraphQL extension

If you use VSCode, install its `GraphQL` extension.
Then open the directory `Nexar.Client` in VSCode and edit `Resources/*.graphql` files.
VSCode reads the configuration and schema and provides GraphQL syntax highlighting, schema validation, code completion and tooltips.

#### Banana Cake Pop web IDE

Navigate to <https://api.nexar.com/graphql/> in your browser and you get the IDE for Nexar GraphQL.
Compose queries in the IDE and use in `*.graphql` files (copy/paste, unlike with VSCode).

In order to try queries in the IDE, set the HTTP header `token` to your Nexar token.
If operations require variables, use the variables tab for defining their values.

### Update Nexar GraphQL schema

Nexar GraphQL API is being actively developed and its GraphQL schema evolves.
The schema packaged with installed templates may be slightly out of date.
Use the StrawberryShake tools for updating the schema `Nexar.Client/schema.graphql`.

You may install the tools globally:

    dotnet tool install StrawberryShake.Tools --global

And then update the schema like this:

    cd Nexar.Client
    dotnet-graphql update

Alternatively, install the tools locally (suitable for source control):

    cd <repository-root>
    dotnet new tool-manifest
    dotnet tool install StrawberryShake.Tools

On another machine you may restore local tools by:

    dotnet tool restore

To update the schema with restored local tools:

    cd Nexar.Client
    dotnet graphql update

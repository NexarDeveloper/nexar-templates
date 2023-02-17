# Nexar.Templates

[nexar.com]: https://nexar.com/
[StrawberryShake]: https://github.com/ChilliCream/hotchocolate

Nexar client dotnet templates.

[nuget.org/packages/Nexar.Templates](https://www.nuget.org/packages/Nexar.Templates/)

## Install

In order to install the templates, invoke:

```
dotnet new install Nexar.Templates
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
dotnet new uninstall Nexar.Templates
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

### Update GraphQL schema

Nexar GraphQL API is being actively developed and its GraphQL schema evolves.
The schema packaged with templates may be out of date.

Ensure `StrawberryShake.Tools` (listed in `.config\dotnet-tools.json`).
This step is needed once to install the package.

    cd Nexar.Client
    dotnet tools restore

Update the Nexar GraphQL schema (`schema.graphql`).

    cd Nexar.Client
    dotnet graphql update

The above commands use [StrawberryShake.Tools](https://www.nuget.org/packages/StrawberryShake.Tools).
The recommended version is 12.17.0 (do not use 13+ yet, it has a known issue).

### Compose GraphQL

Modify or replace the sample query in `Nexar.Client/Resources`.
Build the solution. StrawberryShake regenerates the query and result types.
At this point `Program.cs` using the old query and result may not compile.
Update its code for the new query and result types.
Build and run the updated program.

You can add new GraphQL operations (queries, mutations) to the same file or
create additional `*.graphql` files. Anonymous operations are not supported,
names are used for the corresponding generated C# types names. Use unique
operation names: `query GetSomething ...` or `mutation DoSomething ...`.

### GraphQL IDEs

Composing queries in `*.graphql` files using plain text editors is possible but tedious and error prone.
Instead or in addition, consider using one of GraphQL IDEs, for example one of these:

#### Banana Cake Pop (web)

Navigate to <https://api.nexar.com/graphql/> in your browser and you get the IDE for Nexar GraphQL.
Compose queries in the IDE and use in `*.graphql` files (copy/paste, unlike with desktop IDEs).

In order to try queries in the IDE, use the tab `HTTP headers` and set `Authorization` to `Bearer <nexar-token>`.
If operations require variables, use the tab `Variables` and define variables using JSON format.

#### Visual Studio extension

If you use Visual Studio, try [Strawberry Shake (preview)](https://marketplace.visualstudio.com/items?itemName=ChilliCream.strawberryshake-visualstudio).

It is from the same vendor as `StrawberryShake.Tools`. It understands the
client configuration file and schema without additional steps. It provides
syntax highlighting, schema validation, code completion, tooltips.

In spite of the preview status, the extension works very well with just a few
minor issues. One of them: it creates an extra `.graphqlrc.json` file in the
solution directory. Exclude this noise file from your source control.

#### VSCode GraphQL extensions

If you use VSCode, there are several handy GraphQL extensions, per your choice.
They read some configuration with registered `schema.graphql` and provide
GraphQL syntax highlighting, schema validation, code completion, tooltips.
Some even invoke GraphQL operations in VSCode and show results.

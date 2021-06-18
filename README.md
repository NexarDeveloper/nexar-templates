# Nexar.Templates

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
For getting the token, it starts the browser for signing in.

    dotnet new nexar-console-design

### nexar-console-supply

C# console app running the loop of queries `supSearchMpn`.
For getting the token, it requires environment variables:
`NEXAR_CLIENT_ID` and `NEXAR_CLIENT_SECRET`.

    dotnet new nexar-console-supply

## Uninstall

In order to uninstall the templates, invoke:

```
dotnet new -u Nexar.Templates
```

## Example

Having installed Nexar templates, try the following steps:

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

[StrawberryShake]: https://github.com/ChilliCream/hotchocolate

- The [StrawberryShake] processes queries in `Resources` and generates C# client code in `Generated`.
- `Program.cs` is compiled with generated strongly typed query execution and results.
- When the app runs:
    - it starts the browser for signing in and getting the token
    - it creates and configures the Nexar client with the token
    - it runs the query and prints the results

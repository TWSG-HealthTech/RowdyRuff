# Server Project for CATE

![alt Build Status](https://ci.appveyor.com/api/projects/status/github/TWSG-HealthTech/RowdyRuff?branch=master&retina=true "Build Status")

<a href="https://ci.appveyor.com/project/TWSGHealthTech/RowdyRuff" target="_blank">Latest Build</a>

## Set Up

- Project requires Visual Studio 2015 (at least Update 3), .NET Core 1.0.0 - VS 2015 Tooling Preview 2 (https://www.microsoft.com/net/core#windows)
- Download and install SQL Server Express and Tools (ExpressAndTools 64BIT\SQLEXPRWT_x64_ENU.exe) from https://www.microsoft.com/en-sg/download/details.aspx?id=42299
    - When installing, choose Mixed Mode authentication. The project by default uses Windows Authentication for SQL Server

### Secrets file
The project contains a secrets file that is encrypted.  In order to run the project you will need to decrypt this file.
Run this command:
```
./build.ps1 -target Decrypt -secret=<<the secret>>
```
Ask the CATE/RowdyRuff team lead for the secret.

Alternatively, if you are not in the CATE/RowdyRuff team (and hence the team lead won't tell you the secret) you can create your own `appsettings.json` file with the following template:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLExpress;Database=RowdyRuff;Integrated Security=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "BahmniConnection": {
    "Username": <<Bahmni API username>>,
    "Password": <<Bahmni API password>>  
  } 
}

```

## Solution Structure

This is ASP.NET Core application (target .NET framework 4.6.1)
- `RowdyRuff`: main ASP.NET UI project. Each feature-related functions are organized into different areas, .e.g. `Main` and `Video`:
```
Areas
    Main
        Controllers
        Views
    Video
        Controllers
        Views
```
- `RowdyRuff.Core`: containing model classes
- `RowdyRuff.Repository`: persistence layer, containing Entity Framework related code, mapping model classes to database tables
- `RowdyRuff.Common`: common classes

`Note`: At the moment `RowdyRuff` project has a postcompile command to copy all the output dll of `RowdyRuff.Repository` project to its output directory. The reason is because `RowdyRuff` doesn't have any dependency to `RowdyRuff.Repository`, however at runtime, it needs `RowdyRuff.Repository` dlls to run properly

## Technical Details

### Startup configuration

When the application is started up, methods in `Startup` class of `RowdyRuff` project are invoked to initialize the UI layer. `Startup` class will use `BootstrapperLoader` to dynamically find all `Bootstrapper` classes in any dlls startingwith `RowdyRuff...` and invoke methods in those bootstrapper classes to initialize other layers. 
The reason is to avoid putting everything (.e.g. database initialization code) in `Startup` (and to avoid UI project to have reference to projects that it doesn't need, .e.g. Repository)

The flow is as follow:

```
Startup constructor is called
    Create BootstrapperLoader
    BootstrapperLoader looks for all dlls starting with RowdyRuff in current executing diretory that contains at least one class with name Bootstrapper
    BootstrapperLoader creates instances of those Bootstrapper class. Bootstrapper class must have either default constructor or a constructor that accepts single parameter of type IConfigurationRoot

Startup ConfigureServices() is called
    BootstrapperLoader looks for any ConfigureService(IServiceCollection) method in bootstrapper classes found above and invoke those

Startup Configure() is called
    BootstrapperLoader looks for Configure(...) or ConfigureDevelopment(...) methods in bootstrapper classes and invoke those. Configure() and ConfigureDevelopment() can inject any dependencies that it needs, as long as it's registered with IServiceCollection. This is similar to how Startup class works at the moment
```

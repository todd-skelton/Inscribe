[![](https://img.shields.io/nuget/v/Inscribe.svg)](https://www.nuget.org/packages/Inscribe) [![](https://img.shields.io/nuget/vpre/Inscribe.svg)](https://www.nuget.org/packages/Inscribe)

# Inscribe
Logging provider abstraction for .NET's logging API—includes email and Entity Framework Core providers.

## Installation
### Package Manager
`Install-Package Inscribe`

### .NET CLI
`dotnet add package Inscribe`

### Inscribe Email

##### Sample appsettings.json
Your appsettings for email logging should look something like this.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    },
    "Email": {
      "LogLevel": {
        "Default": "Critical"
      },
      "ApplicationName": "Your Application",
      "Smtp": {
        "Host": "smtp.gmail.com",
        "Credentials": {
          "Username": "your_email@gmail.com",
          "Password": "Password!"
        }
      },
      "From": {
        "Address": "your_email@gmail.com",
        "DisplayName": "Your Name"
      },
      "To": [
        {
          "Address": "your_email@gmail.com",
          "DisplayName": "Your Name"
        }
      ]
    }
  }
}
```

#### Sample Configure Services Startup.cs

Adding your email logger is just as easy as calling `.AddEmail()` to the built-in .NET Core logging configuration.

```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Other service configuration...
        
        services.AddLogging(builder => builder.AddEmail());
    }
}
```

#### Using the logging

Please check out the offical documentation on .NET Core logger for information on how to use the logger. This library is just a provider.

[https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging)
# Marten.AspNetCore, Marten.AspNetCore.Mvc [![Build status](https://ci.appveyor.com/api/projects/status/y06wrff4yrelm847?svg=true)](https://ci.appveyor.com/project/jokokko/marten-aspnetcore) [![NuGet Version](http://img.shields.io/nuget/v/Marten.AspNetCore.svg?style=flat)](https://www.nuget.org/packages/Marten.AspNetCore/) [![NuGet Version](http://img.shields.io/nuget/v/Marten.AspNetCore.Mvc.svg?style=flat)](https://www.nuget.org/packages/Marten.AspNetCore.Mvc/)
[ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) & [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/) extensions for [Marten](http://jasperfx.github.io/marten/).

**Packages** [Marten.AspNetCore](https://www.nuget.org/packages/Marten.AspNetCore), [Marten.AspNetCore.Mvc](https://www.nuget.org/packages/Marten.AspNetCore.Mvc) | **Platforms** .NET Standard 2.0

### Example Usage

#### Basic Bootstrap
```csharp
// Bootstrap
public void ConfigureServices(IServiceCollection services)
{    
    services
        // Registers IDocumentStore for DI (scoped to Singleton as per Marten best practices)
        .UseMarten(c => c.Connection("cstring"))
        // Registers IDocumentSession, IQuerySession, IEventStore for DI (default scope Scoped)
        .UseSessions();

    services.AddMvc()
        // Automatically save IDocumentSession changes on request processing
        .WithMartenSessionsSaved(saveOnHttpMethods: new [] { "POST", "PATCH" });
        // Or for IDocumentSession.SaveChangesAsync
        // .WithMartenSessionsSavedAsync(saveOnHttpMethods: new [] { "POST", "PATCH" }			
}
```
Additional optional arguments are documented.
```csharp
// Sample Controller
[Route("api/[controller]")]
[ApiController]
public class DummyDocumentSessionController : ControllerBase
{
    private readonly IDocumentSession session;

    public DummyDocumentSessionController(IDocumentSession session)
    {
        this.session = session;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Dummy>> Get()
    {
        // Access to scoped session
        return session.Query<Dummy>().Where(x => x.Value != null).ToList();
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        var o = new Dummy { Value = value };
        // Changes are automatically saved after the request has been processed
        session.Insert(o);
    }
}
```

#### Using `IServiceProvider` in the store configuration

Some of your Document Store configuration might rely on injected services. Therefore you might need to have access to the service provider instance.
Following example shows how to achieve this.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IImortantService, ImportantService>();
    services
        // Registers IDocumentStore for DI (scoped to Singleton as per Marten best practices)
        .UseMarten((serviceProvider, options) =>
            {
                var importantServiceInstance = serviceProvider.GetService<IImortantService>();
                options.Connection("connection string");
                options.Events.InlineProjections.Add(new ImportantProjection(importantServiceInstance));
            })
        // Registers IDocumentSession, IQuerySession, IEventStore for DI (default scope Scoped)
        .UseSessions();

    services.AddMvc()
        // Automatically save IDocumentSession changes on request processing
        .WithMartenSessionsSaved(saveOnHttpMethods: new [] { "POST", "PATCH" });
        // Or for IDocumentSession.SaveChangesAsync
        // .WithMartenSessionsSavedAsync(saveOnHttpMethods: new [] { "POST", "PATCH" }			
}
```

Note: This is a contributor project.

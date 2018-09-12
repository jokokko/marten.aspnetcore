using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Marten;
using Marten.AspNetCore.Tests.Harness;

namespace Marten.AspNetCore.Playground
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		
		public void ConfigureServices(IServiceCollection services)
		{
			services
				// Registers IDocumentStore for DI (scoped to Singleton as per Marten best practices)
				.UseMarten(c => c.Connection(ConnectionstringProvider.Get()))
				// Registers IDocumentSession, IQuerySession for DI (default scope Scoped)
				.UseSessions();

			services.AddMvc()
				// Automatically save IDocumentSession changes on request processing
				.WithMartenSessionsSaved(saveOnHttpMethods: new[] { "POST", "PATCH" });
			// Or for IDocumentSession.SaveChangesAsync
			// .WithMartenSessionsSavedAsync(saveOnHttpMethods: new [] { "POST", "PATCH" })			
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}
	}
}

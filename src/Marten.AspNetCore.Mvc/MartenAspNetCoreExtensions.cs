using System.Collections.Generic;
using Marten.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Marten
{
	// ReSharper disable once InconsistentNaming
	public static class MartenAspNetCoreMVCExtensions
	{
		/// <summary>
		/// Registers Marten to save IDocumentSession changes on request processing.
		/// </summary>
		/// <remarks>Assumes that IDocumentSession is registered &amp; resolvable. Can be paired with UseMarten in Marten.AspNetCore</remarks>
		/// <param name="mvcBuilder">ASP.NET Core MVC configurator.</param>
		/// <param name="skipSaveOnSessionExceptions">Skip saving session changes if action or a subsequent filter threw an exception.</param>
		/// <param name="saveOnHttpMethods">Restrict HTTP methods on which changes are saved.</param>		
		public static IMvcBuilder WithMartenSessionsSaved(this IMvcBuilder mvcBuilder, bool skipSaveOnSessionExceptions = true, IEnumerable<string> saveOnHttpMethods = null)
		{
			mvcBuilder.AddMvcOptions(o =>
			{								
				o.Filters.Add<MartenSaveSessionActionFilter>();
			});

			var settings = new MartenSaveSessionActionFilterConfiguration(skipSaveOnSessionExceptions, saveOnHttpMethods);

			mvcBuilder.Services.Add(new ServiceDescriptor(typeof(MartenSaveSessionActionFilterConfiguration), settings));

			return mvcBuilder;
		}

		/// <summary>
		/// Registers Marten to save IDocumentSession changes on request processing. Async counterpart to WithMartenSessionsSaved, using IDocumentSession.SaveChangesAsync.
		/// </summary>
		/// <param name="mvcBuilder">ASP.NET Core MVC configurator.</param>
		/// <param name="skipSaveOnSessionExceptions">Skip saving session changes if action or a subsequent filter threw an exception.</param>
		/// <param name="saveOnHttpMethods">Restrict HTTP methods on which changes are saved.</param>
		public static IMvcBuilder WithMartenSessionsSavedAsync(this IMvcBuilder mvcBuilder, bool skipSaveOnSessionExceptions = true, IEnumerable<string> saveOnHttpMethods = null)
		{
			mvcBuilder.AddMvcOptions(o =>
			{
				o.Filters.Add<MartenAsyncSaveSessionActionFilter>();
			});

			var settings = new MartenSaveSessionActionFilterConfiguration(skipSaveOnSessionExceptions, saveOnHttpMethods);

			mvcBuilder.Services.Add(new ServiceDescriptor(typeof(MartenSaveSessionActionFilterConfiguration), settings));

			return mvcBuilder;
		}
	}
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marten.AspNetCore.Mvc
{
	public sealed class MartenSaveSessionActionFilter : IActionFilter
	{
		private readonly IDocumentSession session;
		private readonly MartenSaveSessionActionFilterConfiguration configuration;

		public MartenSaveSessionActionFilter(IDocumentSession session, MartenSaveSessionActionFilterConfiguration configuration)
		{
			this.session = session ?? throw new ArgumentNullException(nameof(session));
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}


		public void OnActionExecuting(ActionExecutingContext context)
		{						
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (configuration.SkipSaveOnSessionExceptions && context.Exception != null)
			{
				return;
			}

			if (configuration.SaveOnHttpMethods == null)
			{
				session.SaveChanges();
				return;
			}

			if (configuration.SaveOnHttpMethods.Any(x => x.Equals(context.HttpContext.Request.Method)))
			{
				session.SaveChanges();
			}
			
		}
	}
}
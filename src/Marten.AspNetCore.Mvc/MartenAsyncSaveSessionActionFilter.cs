using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marten.AspNetCore.Mvc
{
	public sealed class MartenAsyncSaveSessionActionFilter : IAsyncActionFilter
	{
		private readonly IDocumentSession session;
		private readonly MartenSaveSessionActionFilterConfiguration configuration;

		public MartenAsyncSaveSessionActionFilter(IDocumentSession session, MartenSaveSessionActionFilterConfiguration configuration)
		{
			this.session = session ?? throw new ArgumentNullException(nameof(session));
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var result = await next();

			if (configuration.SkipSaveOnSessionExceptions && result.Exception != null)
			{
				return;
			}

			if (configuration.SaveOnHttpMethods == null)
			{
				await session.SaveChangesAsync();
				return;
			}

			if (configuration.SaveOnHttpMethods.Any(x => x.Equals(context.HttpContext.Request.Method)))
			{
				await session.SaveChangesAsync();
			}
		}
	}
}
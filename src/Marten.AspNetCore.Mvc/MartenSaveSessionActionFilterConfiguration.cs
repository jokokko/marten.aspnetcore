using System.Collections.Generic;
using System.Linq;

namespace Marten.AspNetCore.Mvc
{
	public sealed class MartenSaveSessionActionFilterConfiguration
	{
		public bool SkipSaveOnSessionExceptions { get; }
		public string[] SaveOnHttpMethods { get; }

		public MartenSaveSessionActionFilterConfiguration(bool skipSaveOnSessionExceptions = true, IEnumerable<string> saveOnHttpMethods = null)
		{
			SkipSaveOnSessionExceptions = skipSaveOnSessionExceptions;
			SaveOnHttpMethods = saveOnHttpMethods?.ToArray();
		}
	}
}
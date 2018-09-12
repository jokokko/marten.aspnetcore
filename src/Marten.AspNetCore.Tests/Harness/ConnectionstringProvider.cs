using System;

namespace Marten.AspNetCore.Tests.Harness
{
	public static class ConnectionstringProvider
	{
		public static string Get()
		{
			var env = Environment.GetEnvironmentVariable("marten_testing_database");

			if (string.IsNullOrEmpty(env))
			{
				throw new InvalidOperationException("marten_testing_database environment variable not set (e.g. marten_testing_database=host=localhost;database=marten_test;password=marten;username=marten).");
			}

			return env;
		}
	}
}
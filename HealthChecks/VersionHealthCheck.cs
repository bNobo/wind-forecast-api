using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace wind_forecast_api.HealthChecks
{
	public class VersionHealthCheck : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			return Task.Run(GetVersionHealthCheckResult);
		}

		private static HealthCheckResult GetVersionHealthCheckResult()
		{
			var version = Assembly.GetEntryAssembly().GetName().Version;

			return HealthCheckResult.Healthy(
				$"v{version?.Major}.{version?.Minor}.{version?.Build}"
				);
		}
	}
}

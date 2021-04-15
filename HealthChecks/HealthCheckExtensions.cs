using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.HealthChecks
{
	public static class HealthCheckExtensions
	{
		public static IEndpointConventionBuilder MapHealthChecks(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
		{
			return endpoints.MapHealthChecks("/health",
				new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
				{
					ResponseWriter = JsonHealthCheckResponseWriter.WriteResponse,
					ResultStatusCodes =
					{
						[HealthStatus.Healthy] = StatusCodes.Status200OK,
						[HealthStatus.Degraded] = StatusCodes.Status200OK,
						[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
					},
					AllowCachingResponses = false
				});
		}

		public static string ToJsonString(this HealthReport result)
		{
			var json = new JObject(
				new JProperty("status", result.Status.ToString()),
				new JProperty("results", new JObject(result.Entries.Select(pair =>
					new JProperty(pair.Key, new JObject(
						new JProperty("status", pair.Value.Status.ToString()),
						new JProperty("description", pair.Value.Description),
						new JProperty("data", new JObject(pair.Value.Data.Select(
							p => new JProperty(p.Key, p.Value))))))))));

			return json.ToString(Formatting.Indented);
		}
	}
}

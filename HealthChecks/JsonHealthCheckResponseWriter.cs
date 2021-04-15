using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.HealthChecks
{
	public static class JsonHealthCheckResponseWriter
	{
		public static Task WriteResponse(HttpContext context, HealthReport result)
		{
			context.Response.ContentType = "application/json";

			return context
				.Response
				.WriteAsync(
					result.ToJsonString()
					);
		}
	}
}

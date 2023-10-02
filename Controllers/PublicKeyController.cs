using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wind_forecast_api.Options;

namespace wind_forecast_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicKeyController : ControllerBase
    {
        private readonly PushNotificationsOptions _options;

        public PublicKeyController(IOptions<PushNotificationsOptions> options)
        {
            _options = options.Value;
        }

        [ResponseCache(Duration = 86400)]
        public ContentResult Get()
        {
            return Content(_options.PublicKey, "text/plain");
        }
    }
}

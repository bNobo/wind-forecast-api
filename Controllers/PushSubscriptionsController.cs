using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wind_forecast_api.Services;

namespace wind_forecast_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;

        public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
        }

        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            _pushSubscriptionsService.Insert(subscription);
        }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(endpoint);
        }
    }
}

using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wind_forecast_api.Services;

namespace wind_forecast_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;

        public PushSubscriptionsController(
            IPushSubscriptionsService pushSubscriptionsService,
            PushServiceClient pushClient)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;
        }

        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            _pushSubscriptionsService.Insert(subscription);
            SendNotification(subscription);
        }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(endpoint);
        }

        private void SendNotification(PushSubscription subscription)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = "You are registered",
                Body = $"You're subscription has been registered and you will be notified in case of strong wind forecast",
                Icon = "assets/icons/icon-96x96.png"
            }.ToPushMessage();

            _pushClient.RequestPushMessageDeliveryAsync(subscription, notification, CancellationToken.None);
        }

    }
}

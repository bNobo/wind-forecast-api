using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wind_forecast_api.Options;
using wind_forecast_api.Services;
using static wind_forecast_api.AngularPushNotification;

namespace wind_forecast_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindAlertController
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;
        private readonly ILogger<WindAlertController> _logger;
        private readonly IConfiguration _configuration;

        public WindAlertController(
            IOptions<PushNotificationsOptions> options, 
            IPushSubscriptionsService pushSubscriptionsService, 
            PushServiceClient pushClient,
            ILogger<WindAlertController> logger,
            IConfiguration configuration
            )
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;
            _logger = logger;
            _configuration = configuration;
            _logger.LogInformation("WindAlertController ctor");

            var vapidAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                Subject = options.Value.Subject
            };

            _pushClient.DefaultAuthentication = vapidAuthentication;
        }

        /// <summary>
        /// Send wind alert to every subscribers
        /// </summary>
        /// <param name="velocity"></param>
        public void Post(int velocity)
        {
            SendNotifications(velocity, CancellationToken.None);
        }

        private void SendNotifications(int velocity, CancellationToken stoppingToken)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = "Strong wind alert! (dummy)",
                Body = $"Wind velocity: {velocity}",
                Icon = "assets/icons/icon-96x96.png",
                Actions = new List<NotificationAction> { new NotificationAction("openwebsite", "Open webapp") },
                Data = new Dictionary<string, object> { { "url", _configuration["FrontUrl"] } },
            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // fire-and-forget
                _pushClient.RequestPushMessageDeliveryAsync(subscription, notification, stoppingToken);
            }
        }
    }
}

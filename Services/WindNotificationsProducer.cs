using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wind_forecast_api.Options;

namespace wind_forecast_api.Services
{
    public class WindNotificationsProducer : BackgroundService
    {
        private const int NOTIFICATION_FREQUENCY = 60000;
        private readonly Random _random = new Random();
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;

        public WindNotificationsProducer(IOptions<PushNotificationsOptions> options, IPushSubscriptionsService pushSubscriptionsService, PushServiceClient pushClient)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;

            var vapidAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                // URL identifiant l'application
                Subject = options.Value.Subject
            };

            _pushClient.DefaultAuthentication = vapidAuthentication;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(NOTIFICATION_FREQUENCY, stoppingToken);
                SendNotifications(_random.Next(-20, 55), stoppingToken);
            }
        }

        private void SendNotifications(int temperatureC, CancellationToken stoppingToken)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = "New Weather Forecast",
                Body = $"Temp. (C): {temperatureC} | Temp. (F): {32 + (int)(temperatureC / 0.5556)}",
                Icon = "assets/icons/icon-96x96.png"
            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // fire-and-forget
                _pushClient.RequestPushMessageDeliveryAsync(subscription, notification, stoppingToken);
            }
        }
    }
}

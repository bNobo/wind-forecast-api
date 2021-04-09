using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace wind_forecast_api.Services
{
    public class WindNotificationsProducer : BackgroundService
    {
        // Every 3 hours
        private const int NOTIFICATION_FREQUENCY = 60000;//3*60*60*1000;
        private const string RequestUri = "https://wttr.in/Montpellier?format=j1";
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;
        private readonly HttpClient _httpClient;

        public WindNotificationsProducer(
            IPushSubscriptionsService pushSubscriptionsService, 
            PushServiceClient pushClient,
            HttpClient httpClient)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int maxAllowedSpeed = 20;
            int maxSpeed = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(NOTIFICATION_FREQUENCY, stoppingToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(RequestUri);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var weathers = json["weather"];
                    var tomorrow = DateTime.Today.AddDays(1);

                    foreach (var weather in weathers)
                    {
                        var date = DateTime.Parse(weather["date"].Value<string>()).Date;

                        if (date == tomorrow)
                        {
                            var hourlies = weather["hourly"];
                            var windspeeds = hourlies.Select(data => int.Parse(data["windspeedKmph"].Value<string>()));
                            maxSpeed = windspeeds.Max();
                            break;
                        }
                    }

                    if (maxSpeed > maxAllowedSpeed)
                    {
                        SendNotifications(maxSpeed, stoppingToken);
                    }
                }
                else
                {
                    Console.WriteLine(httpResponseMessage.StatusCode);
                    Console.WriteLine(httpResponseMessage.ReasonPhrase);
                }
            }
        }

        private void SendNotifications(int velocity, CancellationToken stoppingToken)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = "Strong wind alert !",
                Body = $"Tomorrow forecast : {velocity} Km/h",
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

using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wind_forecast_api.Services
{
    public class PushNotificationsProducer : BackgroundService
    {
        // Every 3 hours
#if DEBUG
        private const int NOTIFICATION_FREQUENCY = 30 * 1000;
#else
        private const int NOTIFICATION_FREQUENCY = 3*60*60*1000;
#endif
        private const string RequestUri = "https://wttr.in/Montpellier?format=j1";
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PushNotificationsProducer> _logger;

        public PushNotificationsProducer(
            IPushSubscriptionsService pushSubscriptionsService, 
            PushServiceClient pushClient,
            HttpClient httpClient,
            ILogger<PushNotificationsProducer> logger)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;
            _httpClient = httpClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("ExecuteAsync, Culture = {0}", System.Threading.Thread.CurrentThread.CurrentCulture);
                await ExecuteInternalAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        private async Task ExecuteInternalAsync(CancellationToken stoppingToken)
        {
            const int maxAllowedWindGust = 30;
            const int maxAllowedChanceOfRain = 40;
            const double maxAllowedPrecipMM = 0.1;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("download weather forecast");
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(RequestUri);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation("reading and parsing content");
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var weathers = json["weather"];
                    var tomorrow = DateTime.Today.AddDays(1);
                    int maxWindGust = 0;
                    int maxChanceOfRain = 0;
                    double maxPrecipMM = 0;

                    foreach (var weather in weathers)
                    {
                        var date = DateTime.Parse(weather["date"].Value<string>()).Date;

                        if (date == tomorrow)
                        {
                            var hourlies = weather["hourly"];
                            var windGusts = hourlies.Select(data => data["WindGustKmph"].Value<int>());
                            var chanceOfRains = hourlies.Select(data => data["chanceofrain"].Value<int>());
                            var precipMMs = hourlies.Select(data => { _logger.LogInformation("precipMM = {0}", data["precipMM"].Value<double>()); return data["precipMM"].Value<double>(); });
                            maxWindGust = windGusts.Max();
                            maxChanceOfRain = chanceOfRains.Max();
                            maxPrecipMM = precipMMs.Max();
                            break;
                        }
                    }

                    // 💪 body builder 
                    var bodyBuilder = new StringBuilder();
                    string title = "Weather alert tomorrow!";

                    if (maxWindGust > maxAllowedWindGust)
                    {
                        bodyBuilder.AppendFormat("Expected wind gusts up to {0} Km/h", maxWindGust);
                        bodyBuilder.AppendLine();
                    }

                    if (maxChanceOfRain > maxAllowedChanceOfRain && maxPrecipMM > maxAllowedPrecipMM)
                    {
                        bodyBuilder.AppendFormat("Chance of rain is {0} %. Precipitation forecast : {1} millimeters", maxChanceOfRain, maxPrecipMM);
                    }

                    if (bodyBuilder.Length > 0)
                    {
                        SendNotifications(title, bodyBuilder.ToString(), stoppingToken);
                    }
                }
                else
                {
                    _logger.LogError("{0} - {1}", httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                }

                _logger.LogInformation("wait for {0} ms", NOTIFICATION_FREQUENCY);
                await Task.Delay(NOTIFICATION_FREQUENCY, stoppingToken);
            }
        }

        public void SendNotifications(string title, string body, CancellationToken stoppingToken)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = title,
                Body = body,
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

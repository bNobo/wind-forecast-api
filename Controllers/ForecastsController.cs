using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using wind_forecast_api.Models;

namespace wind_forecast_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastsController : ControllerBase
    {
        private const string RequestUri = "https://wttr.in/Montpellier?format=j1";

        private readonly ILogger<ForecastsController> _logger;
        private readonly HttpClient _httpClient;

        private Forecast forecast = new Forecast
        {
            Id = "tomorrow",
            Weathers = new List<Weather>
                {
                    new Weather { Title = "Morning" },
                    new Weather { Title = "Afternoon" },
                    new Weather { Title = "Evening" },
                    new Weather { Title = "Night" }
                }
        };

        public ForecastsController(
            ILogger<ForecastsController> logger,
            HttpClient httpClient
            )
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Get(string id)
        {
            if (id != "tomorrow")
            {
                return NoContent();
            }

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(RequestUri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                var weathers = json["weather"];
                var tomorrow = DateTime.Today.AddDays(1);
                var dayAfterTomorrow = tomorrow.AddDays(1);

                foreach (var weather in weathers)
                {
                    var date = DateTime.Parse(weather["date"].Value<string>()).Date;

                    if (date == tomorrow)
                    {
                        var hourlies = weather["hourly"];
                        SetWeatherSymbolOnDayPart("Morning", hourlies);
                        SetWeatherSymbolOnDayPart("Afternoon", hourlies);
                        SetWeatherSymbolOnDayPart("Evening", hourlies);                        
                    }
                    else if (date == dayAfterTomorrow)
                    {
                        var hourlies = weather["hourly"];
                        SetWeatherSymbolOnDayPart("Night", hourlies);
                    }
                }
            }
            
            return Ok(forecast);
        }

        private void SetWeatherSymbolOnDayPart(string dayPartName, JToken hourlies)
        {
            string time;

            switch (dayPartName)
            {
                case "Morning":
                    time = "600";
                    break;
                case "Afternoon":
                    time = "1200";
                    break;
                case "Evening":
                    time = "1800";
                    break;
                case "Night":
                    time = "0";
                    break;
                default:
                    throw new ArgumentException($"Unexpected dayPartName {dayPartName}", nameof(dayPartName));
            }

            var dayPart = hourlies.Single(data => data["time"].Value<string>() == time);
            var weatherCode = dayPart["weatherCode"].Value<string>();
            var weatherSymbolName = Constants.WeatherCodes[weatherCode];
            var weatherSymbol = Constants.WeatherSymbols[weatherSymbolName];
            forecast.Weathers.Single(_ => _.Title == dayPartName).WeatherSymbol = weatherSymbol;
        }
    }
}

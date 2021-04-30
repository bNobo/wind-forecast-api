﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.Models
{
    public static class Constants
    {
        public static readonly Dictionary<string, string> WeatherCodes = new Dictionary<string, string>
        {
            { "113", "Sunny" },
            { "116", "PartlyCloudy" },
            { "119", "Cloudy" },
            { "122", "VeryCloudy" },
            { "143", "Fog" },
            { "176", "LightShowers" },
            { "179", "LightSleetShowers" },
            { "182", "LightSleet" },
            { "185", "LightSleet" },
            { "200", "ThunderyShowers" },
            { "227", "LightSnow" },
            { "230", "HeavySnow" },
            { "248", "Fog" },
            { "260", "Fog" },
            { "263", "LightShowers" },
            { "266", "LightRain" },
            { "281", "LightSleet" },
            { "284", "LightSleet" },
            { "293", "LightRain" },
            { "296", "LightRain" },
            { "299", "HeavyShowers" },
            { "302", "HeavyRain" },
            { "305", "HeavyShowers" },
            { "308", "HeavyRain" },
            { "311", "LightSleet" },
            { "314", "LightSleet" },
            { "317", "LightSleet" },
            { "320", "LightSnow" },
            { "323", "LightSnowShowers" },
            { "326", "LightSnowShowers" },
            { "329", "HeavySnow" },
            { "332", "HeavySnow" },
            { "335", "HeavySnowShowers" },
            { "338", "HeavySnow" },
            { "350", "LightSleet" },
            { "353", "LightShowers" },
            { "356", "HeavyShowers" },
            { "359", "HeavyRain" },
            { "362", "LightSleetShowers" },
            { "365", "LightSleetShowers" },
            { "368", "LightSnowShowers" },
            { "371", "HeavySnowShowers" },
            { "374", "LightSleetShowers" },
            { "377", "LightSleet" },
            { "386", "ThunderyShowers" },
            { "389", "ThunderyHeavyRain" },
            { "392", "ThunderySnowShowers" },
            { "395", "HeavySnowShowers" }
        };
        
        public static readonly Dictionary<string, string> WeatherSymbols = new Dictionary<string, string>
        {
            { "Unknown", "⯑" },
            { "Cloudy", "☁" },
            { "Fog", "🌫" },
            { "HeavyRain", "🌧" },
            { "HeavyShowers", "🌧" },
            { "HeavySnow", "❄" },
            { "HeavySnowShowers", "❄" },
            { "LightRain", "🌦" },
            { "LightShowers", "🌦" },
            { "LightSleet", "🌨" },
            { "LightSleetShowers", "🌨" },
            { "LightSnow", "🌨" },
            { "LightSnowShowers", "🌨" },
            { "PartlyCloudy", "🌤" },
            { "Sunny", "☀" },
            { "ThunderyHeavyRain", "⛈" },
            { "ThunderyShowers", "⛈" },
            { "ThunderySnowShowers", "⛈" },
            { "VeryCloudy", "☁" }
        };
    }
}

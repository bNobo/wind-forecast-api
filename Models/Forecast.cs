using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.Models
{
    public class Forecast
    {
        public string Id { get; set; }
        public IList<Weather> Weathers { get; set; }
    }
}

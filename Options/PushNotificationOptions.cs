using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.Options
{
    public class PushNotificationsOptions
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Subject { get; set; }
    }
}

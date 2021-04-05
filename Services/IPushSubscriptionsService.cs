using Lib.Net.Http.WebPush;
using System.Collections.Generic;

namespace wind_forecast_api.Services
{
    public interface IPushSubscriptionsService
    {
        void Delete(string endpoint);
        void Insert(PushSubscription subscription);
        IEnumerable<PushSubscription> GetAll();
    }
}
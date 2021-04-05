using Lib.Net.Http.WebPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.Services
{
    public class PushSubscriptionsService : IPushSubscriptionsService
    {
        private readonly List<PushSubscription> _list = new List<PushSubscription>();

        public void Insert(PushSubscription subscription)
        {
            _list.Add(subscription);
        }

        public void Delete(string endpoint)
        {
            _list.RemoveAll(subscription => subscription.Endpoint == endpoint);
        }

        public IEnumerable<PushSubscription> GetAll()
        {
            return _list;
        }
    }
}

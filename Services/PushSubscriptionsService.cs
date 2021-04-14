using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wind_forecast_api.Models;

namespace wind_forecast_api.Services
{
    public class PushSubscriptionsService : IPushSubscriptionsService
    {
        private readonly IMongoCollection<PushSubscription> _collection;


        public PushSubscriptionsService(MongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration["Mongo:DatabaseName"]);
            _collection = database.GetCollection<PushSubscription>(configuration["Mongo:CollectionName"]);
        }

        public void Insert(PushSubscription subscription)
        {
            _collection.ReplaceOne(_ => _.Endpoint == subscription.Endpoint, subscription, new ReplaceOptions { IsUpsert = true });
        }

        public void Delete(string endpoint)
        {
            _collection.DeleteOne(_ => _.Endpoint == endpoint);
        }

        public IEnumerable<MongoPushSubscription> GetAll()
        {
            return _collection.AsQueryable()
                .Select(_ => new MongoPushSubscription
                {
                    Endpoint = _.Endpoint,
                    Keys = _.Keys 
                });
        }
    }
}

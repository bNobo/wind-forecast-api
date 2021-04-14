using Lib.Net.Http.WebPush;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wind_forecast_api.Models
{
    public class MongoPushSubscription : PushSubscription
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get { return Endpoint; } }
    }
}

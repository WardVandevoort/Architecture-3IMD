using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Architecture_3IMD.Models.Domain
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Store_id { get; set; }
        public int Bouquet_id { get; set; }
        public int Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
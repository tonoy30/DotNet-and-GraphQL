using MongoDB.Bson;

namespace GraphQL.API.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
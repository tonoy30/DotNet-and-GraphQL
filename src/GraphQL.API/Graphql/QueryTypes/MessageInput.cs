using MongoDB.Bson;

namespace GraphQL.API.Graphql.Queries
{
    public class MessageInput
    {
        public string Text { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId? ReplyToId { get; set; }
    }
}
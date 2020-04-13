using GraphQL.API.Models;
using GraphQL.API.Repository;
using GreenDonut;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using MongoDB.Bson;

namespace GraphQL.API.Graphql.Queries
{
    public class MessageType:ObjectType<Message>
    {
        protected override void Configure(IObjectTypeDescriptor<Message> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
            descriptor.Field(t => t.Text).Type<NonNullType<StringType>>();
            descriptor.Field("createdBy").Type<NonNullType<UserType>>().Resolver(ctx =>
            {
                var repository = ctx.Service<UserRepository>();
                var dataLoader = ctx.BatchDataLoader<ObjectId, User>("UserById", repository.GetUserAsync);
                return dataLoader.LoadAsync(ctx.Parent<Message>().UserId);
            });
            descriptor.Field("replyTo").Type<MessageType>().Resolver(async ctx =>
            {
                var replyToId = ctx.Parent<Message>().ReplyToId;
                if (replyToId.HasValue)
                {
                    var repository = ctx.Service<MessageRepository>();
                    return await repository.GetMessageById(replyToId.Value);
                }

                return null;
            });
            descriptor.Ignore(t => t.UserId);
            descriptor.Ignore(t => t.ReplyToId);
        }
    }
}
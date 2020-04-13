using GraphQL.API.Models;
using GraphQL.API.Repository;
using GreenDonut;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace GraphQL.API.Graphql.Queries
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field("messages")
                .Resolver(ctx => ctx.Service<MessageRepository>().GetAllMessages())
                .UsePaging<MessageType>()
                .UseFiltering();

            descriptor.Field("usersByCountry")
                .Argument("country", a => a.Type<NonNullType<StringType>>())
                .Type<NonNullType<ListType<NonNullType<UserType>>>>()
                .Resolver(ctx =>
                {
                    var userRepository = ctx.Service<UserRepository>();

                    var userDataLoader =
                        ctx.GroupDataLoader<string, User>(
                            "usersByCountry",
                            userRepository.GetUsersByCountry);

                    return userDataLoader.LoadAsync(ctx.Argument<string>("country"));
                });
        }
    }
}
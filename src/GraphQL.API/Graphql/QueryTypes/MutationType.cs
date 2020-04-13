using HotChocolate.Types;

namespace GraphQL.API.Graphql.Queries
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(t => t.CreateMessageAsync(default, default, default))
                .Argument("messageInput", a => a.Type<NonNullType<MessageInputType>>())
                .Type<MessageType>();
            descriptor.Field(t => t.CreateUserAsync(default, default, default))
                .Argument("userInput", a => a.Type<NonNullType<UserInputType>>())
                .Type<UserType>();
        }
    }
}
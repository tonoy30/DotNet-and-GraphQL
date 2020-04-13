using GraphQL.API.Models;
using HotChocolate.Types;

namespace GraphQL.API.Graphql.Queries
{
    public class UserType:ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
            descriptor.Field(t => t.Name).Type<NonNullType<StringType>>();
        }
    }
}
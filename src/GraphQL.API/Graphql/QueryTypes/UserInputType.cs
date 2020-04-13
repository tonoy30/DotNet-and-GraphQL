using HotChocolate.Types;

namespace GraphQL.API.Graphql.Queries
{
    public class UserInputType:InputObjectType<UserInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UserInput> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(t => t.Name).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.Country).Type<NonNullType<StringType>>();
        }
    }
}
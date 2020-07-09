using HotChocolate.Types;
using PocGraphQLHotChocolate.Infra.Model;
using PocGraphQLHotChocolate.Infra.Resolver;

namespace PocGraphQLHotChocolate.Infra.Types
{
	public class AuthorType : ObjectType<Author>
	{
		protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
		{
			descriptor.Field(a => a.Id).Type<IdType>();
			descriptor.Field(a => a.Name).Type<StringType>();
			descriptor.Field(a => a.Surname).Type<StringType>();
			descriptor.Field<BookResolver>(t => t.GetBooks(default, default));
		}
	}
}

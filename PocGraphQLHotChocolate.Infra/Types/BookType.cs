using HotChocolate.Types;
using PocGraphQLHotChocolate.Infra.Model;
using PocGraphQLHotChocolate.Infra.Resolver;

namespace PocGraphQLHotChocolate.Infra.Types
{
	public class BookType : ObjectType<Book>
	{
		protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
		{
			descriptor.Field(b => b.Id).Type<IdType>();
			descriptor.Field(b => b.Title).Type<StringType>();
			descriptor.Field(b => b.Price).Type<DecimalType>();
			descriptor.Field<AuthorResolver>(t => t.GetAuthor(default, default));
		}
	}
}

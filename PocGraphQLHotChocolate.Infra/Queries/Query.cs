using HotChocolate.Types;
using HotChocolate.Types.Relay;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using PocGraphQLHotChocolate.Infra.Types;
using System.Linq;
using System.Threading.Tasks;

namespace PocGraphQLHotChocolate.Infra.Queries
{
	public class Query
	{
		private readonly IAuthorService _authorService;
		private readonly IBookService _bookService;
		public Query(IAuthorService authorService, IBookService bookService)
		{
			_authorService = authorService;
			_bookService = bookService;
		}
		[UsePaging(SchemaType = typeof(AuthorType))]
		[UseFiltering]
		public IQueryable<Author> Authors => _authorService.GetAllAsync().Result;

		[UsePaging(SchemaType = typeof(BookType))]
		[UseFiltering]
		public IQueryable<Book> Books => _bookService.GetAll();
	}
}

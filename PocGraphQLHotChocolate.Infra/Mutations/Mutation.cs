using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;

namespace PocGraphQLHotChocolate.Infra.Mutations
{
	public class Mutation
    {
		private readonly IBookService _bookService;

		public Mutation(IBookService bookService)
		{
			_bookService = bookService;
		}
		public Book CreateBook(Book book) => _bookService.Add(book);
		public Book DeleteBook(Book book) => _bookService.Remove(book.Id);
	}
}

using HotChocolate;
using HotChocolate.Resolvers;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System.Collections.Generic;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Resolver
{
    public class BookResolver
    {
        private readonly IBookService _bookService;

        public BookResolver([Service]IBookService bookService)
        {
            _bookService = bookService;
        }
        public IEnumerable<Book> GetBooks(Author author, IResolverContext ctx)
        {
            return _bookService.GetAll().Where(b => b.AuthorId == author.Id);
        }
    }
}

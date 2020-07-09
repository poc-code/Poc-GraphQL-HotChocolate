using HotChocolate;
using HotChocolate.Resolvers;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Resolver
{
    public class AuthorResolver
    {
        private readonly IAuthorService _authorService;

        public AuthorResolver([Service]IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public Author GetAuthor(Book book, IResolverContext ctx)
        {
            var data =  _authorService.GetAllAsync().Result;
            return data.Where(a => a.Id == book.AuthorId).FirstOrDefault();
        }
    }
}

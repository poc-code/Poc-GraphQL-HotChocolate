using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }
        public Book Add(Book book) => _repository.Add(book);
        public IQueryable<Book> GetAll() => _repository.GetAll();
        public Book GetById(int id) => _repository.GetById(id);
        public Book Remove(int id) {
            if (id == 0)
                throw new BookNotFoundException() { BookId = id };

            return _repository.Remove(id);
        }
    }
}

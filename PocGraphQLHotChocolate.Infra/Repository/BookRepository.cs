using PocGraphQLHotChocolate.Infra.Context;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Repository
{
    public class BookRepository : IBookRepository
    {
        private DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public Book Add(Book Book)
        {
            var data = _context.Add(Book);
            _context.SaveChanges();
            return data.Entity;
        }

        public IQueryable<Book> GetAll()
        {
            var data = _context.Book;
            return data;
        }

        public Book GetById(int id)
        {
            var data = _context.Book.FirstOrDefault(x => x.Id == id);
            return data;
        }

        public Book Remove(int id)
        {
            var data = _context.Book.FirstOrDefault(x => x.Id == id);
            _context.Remove(data);
            return data;
        }

        public Book Update(int id, Book book)
        {
            var data = _context.Book.FirstOrDefault(x => x.Id == id);

            if(data != null)
            {
                data.Title = book.Title;
                data.Price = book.Price;
            }
            _context.SaveChanges();
            return data;
        }
    }
}

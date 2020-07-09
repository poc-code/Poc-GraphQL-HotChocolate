using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Contracts
{
    public interface IBookService
    {
        IQueryable<Book> GetAll();
        Book GetById(int id);
        Book Remove(int id);
        Book Add(Book book);
    }
}

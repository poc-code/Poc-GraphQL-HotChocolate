using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;
using System.Threading.Tasks;

namespace PocGraphQLHotChocolate.Infra.Contracts
{
    public interface IAuthorRepository
    {
        Task<IQueryable<Author>> GetAll();
        Task<Author> GetById(int id);
        Task<Author> Remove(int id);
        Task<Author> Add(Author author);
        Task<Author> Update(int id, Author author);
    }
}

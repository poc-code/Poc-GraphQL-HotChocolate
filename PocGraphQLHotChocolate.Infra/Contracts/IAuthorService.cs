using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;
using System.Threading.Tasks;

namespace PocGraphQLHotChocolate.Infra.Contracts
{
    public interface IAuthorService
    {
        Task<IQueryable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task<Author> RemoveAsync(int id);
        Task<Author> AddAsync(Author author);
    }
}

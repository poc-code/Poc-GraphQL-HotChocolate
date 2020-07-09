using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System.Linq;
using System.Threading.Tasks;

namespace PocGraphQLHotChocolate.Infra.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _repository;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }
        public async Task<Author> AddAsync(Author author) => await _repository.Add(author);
        public async Task<IQueryable<Author>> GetAllAsync() => await _repository.GetAll();
        public async Task<Author> GetByIdAsync(int id) => await _repository.GetById(id);
        public async Task<Author> RemoveAsync(int id) => await _repository.Remove(id);
    }
}

using Microsoft.EntityFrameworkCore;
using PocGraphQLHotChocolate.Infra.Context;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PocGraphQLHotChocolate.Infra.Repository
{
    public class AuthorRepository : IAuthorRepository
    {

        private DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Author> Add(Author author)
        {
            var data = _context.Add(author);
            await _context.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<IQueryable<Author>> GetAll()
        {
            var data = await Task.Run(() => _context.Author);
            return data;
        }

        public async Task<Author> GetById(int id)
        {
            var data = await _context.Author.FirstOrDefaultAsync(x => x.Id == id);
            return data;
        }

        public async Task<Author> Remove(int id)
        {
            var data = await _context.Author.FirstOrDefaultAsync(x => x.Id.Equals(id));
            _context.Remove(data);
            await _context.SaveChangesAsync();
            return data;

        }

        public async Task<Author> Update(int id, Author author)
        {
            var data = await _context.Author.FirstOrDefaultAsync(x => x.Id.Equals(id));
            data.Name = author.Name;
            data.Surname = author.Surname;
            await _context.SaveChangesAsync();
            return data;
        }
    }
}

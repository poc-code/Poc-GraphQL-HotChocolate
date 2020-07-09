using PocGraphQLHotChocolate.Infra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocGraphQLHotChocolate.Infra.Contracts
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAll();
        Book GetById(int id);
        Book Remove(int id);
        Book Add(Book Book);
        Book Update(int id, Book book);    
    }
}

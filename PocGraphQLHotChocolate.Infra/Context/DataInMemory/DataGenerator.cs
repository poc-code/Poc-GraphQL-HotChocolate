using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PocGraphQLHotChocolate.Infra.Model;
using System;
using System.Linq;

namespace PocGraphQLHotChocolate.Infra.Context.DataInMemory
{
    public static class DataGenerator
    {
        /// <summary>
        /// Inicia banco em memória
        /// </summary>
        public static void Iniciar(IServiceProvider serviceProvider)
        {
            using (var contexto = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                if (contexto.Author.Any())
                {
                    return;   // Dados ja foram providos
                }

                contexto.Author.AddRange(
                    new Author()
                    {
                        Id = 1,
                        Name = "Miguel de Servantes",
                        Surname = "Servantes",
                    },
                    new Author()
                    {
                        Id = 2,
                        Name = "Augusto dos Anjos",
                        Surname = "Anjos",
                    },
                    new Author()
                    {
                        Id = 3,
                        Name = "Fernando Pessoa",
                        Surname = "Pessoa",
                    },
                    new Author()
                    {
                        Id = 4,
                        Name = "George Orwell",
                        Surname = "Orwell",
                    }
                    );

                contexto.Book.AddRange(
                    new Book()
                    {
                        Id = 1,
                        AuthorId = 1,
                        Title = "Dom Quichote de La Mancha",
                        Price = 29.9m
                    },
                    new Book()
                    {
                        Id = 2,
                        AuthorId = 2,
                        Title = "Eu",
                        Price = 25.9m
                    },
                    new Book()
                    {
                        Id = 3,
                        AuthorId = 3,
                        Title = "Livro do Desassossego",
                        Price = 17.9m
                    },
                    new Book()
                    {
                        Id = 4,
                        AuthorId = 3,
                        Title = "Ficcções do Interlúdio",
                        Price = 19.9m
                    },
                    new Book()
                    {
                        Id = 5,
                        AuthorId = 4,
                        Title = "1984",
                        Price = 28.9m
                    },
                    new Book()
                    {
                        Id = 6,
                        AuthorId = 4,
                        Title = "A Revolução dos Bichos",
                        Price = 25.9m
                    }
                    );
                contexto.SaveChanges();
            }
        }
    }
}

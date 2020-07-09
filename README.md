# Poc-GraphQL-HotChocolate

### Referencia
https://www.blexin.com/en-US/Article/Blog/Creating-our-API-with-GraphQL-and-Hot-Chocolate-79
https://github.com/AARNOLD87/GraphQLWithHotChocolate

### Bibiotecas
1. Primeiro vamos criar uma web application, e adicionar as bibliotecas HotChocolate e HotChocolate.AspNetCore com o Nuget package manager. 
A biblioteca HotChocolate.AspNetCore.Playground será usando para fazer teste no browser da api.

### Configuração inicial
#### 2. Configurar o GraphQL no startup da aplicação.
	2.1 ConfigureServices
```csharp
services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddType<AuthorType>()
                .AddType<BookType>()
                .AddQueryType<Query>()
                .Create());
```
#### 2.2 Configure
```csharp
if (env.IsDevelopment())
    {
	app.UsePlayground();
    }

    app.UseGraphQL("/api");
```
### Model Author
3. Adicionar as models.
	3.1 Author
		
```csharp
	public class Author
	{
	    public int Id { get; set; }
	    public string Name { get; set; }
	    public string Surname { get; set; }
	}
```
### Model Book		
3.2 Book
```csharp
public class Book
        {
            public int Id { get; set; }
            public int AuthorId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
        }
```
### Types
4. Definir as Types que serão expostos na api

	4.1 AuthorType
```csharp
	public class AuthorType : ObjectType<Author>
        {
            protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
            {
                descriptor.Field(a => a.Id).Type<IdType>();
                descriptor.Field(a => a.Name).Type<StringType>();
                descriptor.Field(a => a.Surname).Type<StringType>();
            }
        }
```
		
		
#### 4.2 BookType
		
```csharp
	public class BookType : ObjectType<Book>
        {
            protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
            {
                descriptor.Field(b => b.Id).Type<IdType>();
                descriptor.Field(b => b.Title).Type<StringType>();
                descriptor.Field(b => b.Price).Type<DecimalType>();
            }
        }
```
		
No GraphQL o Crud é tratado de forma separada em relação as consultas e inclusões e alterações de dados. As consultas 
são chamadas que Query e inclusões e alterações chamados de Mutation.

Com a anotação UsePaging, estamos instruindo o GraphQL para que os autores retornados pelo serviço tenham que ser disponibilizados 
com paginação e no AuthorType definido anteriormente. Dessa forma, iniciando o aplicativo e indo para o playground, podemos 
fazer a seguinte consulta e ver o resultado.

Com as bibliotecas HotChocolate.Types e HotChocolate.Types.Filters que você pode adicionar ao projeto, você pode adicionar uma nova 
anotação para ativar os filtros.

### Queries	

5. Queries
	5.1 Query	
```csharp
	public class Query
        {
            private readonly IAuthorService _authorService;
            public Query(IAuthorService authorService)
            {
                _authorService = authorService;
            }
            [UsePaging(SchemaType = typeof(AuthorType))]
            [UseFiltering]
            public IQueryable<Author> Authors => _authorService.GetAll();
            
            [UsePaging(SchemaType = typeof(BookType))]
            [UseFiltering]
            public IQueryable<Book> Books => _bookService.GetAll();
        }
```
Adicionamos um novo campo ao esquema BookType com o descritor.Field e pedimos que ele o resolvesse pelo método GetAuthor do AuthorResolver		

### Resolvers
#### 6. Resolver
	6.1 AuthorResolver
```csharp
	public class AuthorResolver
        {
            private readonly IAuthorService _authorService;
         
            public AuthorResolver([Service]IAuthorService authorService)
            {
                _authorService = authorService;
            }
         
            public Author GetAuthor(Book book, IResolverContext ctx)
            {
                return _authorService.GetAll().Where(a => a.Id == book.AuthorId).FirstOrDefault();
            }
        }
```
### Mutation	
Mutation
Para concluir o CRUD com operações de criação e exclusão, precisamos implementar o que é chamado de mutação. É uma classe com métodos que indicam as operações possíveis. Ele deve ser gravado usando a seguinte instrução AddMutationTypez <Mutation> () que deve ser adicionada ao bloco de configuração do GraphQL em Startup.cs

7. Configuração
	7.1 startup
		7.2 ConfigureServices
```csharp
	services.AddGraphQL(s => SchemaBuilder.New()
            .AddServices(s)
            .AddType<AuthorType>()
            .AddType<BookType>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .Create());
```
O serviço precisará criar apenas uma nova instância do livro, adicioná-la à sua lista e retornar o livro criado.	
No playground, podemos testar o novo recurso iniciando a consulta.
		
8. Mutation	
```csharp
{
  public class Mutation
    {
        private readonly IBookService _bookService;
     
        public Mutation(IBookService bookService)
        {
            _bookService = bookService;
        }
     
        public Book CreateBook(CreateBookInput inputBook)
        {
            return _bookService.Create(inputBook);
        }
    }
```
9. Input
	9.1 CreateBookInput
```csharp
	public class CreateBookInput
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int AuthorId { get; set; }
        }
```
		
	
Vamos adicionar agora o recurso de remoção de livros alterando a Mutation existente acrescentando mais um método.
A classe DeleteBookInput possui apenas uma propriedade do tipo int que representa o ID do livro que você deseja excluir.
10. Mutation
```csharp
{
	public class Mutation
	    {
		private readonly IBookService _bookService;

		...

		public Book DeleteBook(DeleteBookInput inputBook)
		{
		    var bookToDelete = _books.Single(b => b.Id == inputBook.Id);
		    _books.Remove(bookToDelete);
		    return bookToDelete;
		}
	    }
```
### Exceções
11. BookNotFoundException 
		
```csharp
	public class BookNotFoundException : Exception
        {
            public int BookId { get; internal set; }
        }

```csharp
	public Book Delete(DeleteBookInput inputBook)
	    {
		var bookToDelete = _books.FirstOrDefault(b => b.Id == inputBook.Id);
		if (bookToDelete == null)
		throw new BookNotFoundException() { BookId = inputBook.Id };
		_books.Remove(bookToDelete);
		return bookToDelete;
	    }
```
12. Alterando a método da mutation de deleção do objeto
	
		
13. IErrorFilter 
	
```csharp
	public class BookNotFoundExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
             if (error.Exception is BookNotFoundException ex)
                 return error.WithMessage($"Book with id {ex.BookId} not found");
                 
             return error;
        }
    }
```
14. Configuração do manipulador de erros no statup	
```csharp
	public void ConfigureServices(IServiceCollection services){
        
        ...
        services.AddErrorFilter<BookNotFoundExceptionFilter>();
        
    }
```
### Requisições
Pesquisar Livro
### Query
[block:code]
{
  "codes": [
    {
      "code": "{\n    books {\n        nodes{\n            id,\n            title,\n            price\n        }\n    }\n}",
      "language": "json"
    }
  ]
}
[/block]

[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/68ae6ae-PostmanSerachBook.jpg",
        "PostmanSerachBook.jpg",
        1491,
        794,
        "#2d2e2f"
      ]
    }
  ]
}
[/block]
Pesquisar Author
### Query
[block:code]
{
  "codes": [
    {
      "code": "{\n    authors{\n        nodes{\n            id,\n            name,\n            surname\n        }\n    }\n}",
      "language": "json"
    }
  ]
}
[/block]
Pesquisar Author com parametro
### Query

[block:code]
{
  "codes": [
    {
      "code": "{\n    authors(where:{\n       name: \"Fernando Pessoa\" \n    }){\n        nodes{\n            id,\n            name,\n            surname\n        }\n    }\n}",
      "language": "json"
    }
  ]
}
[/block]
Pesquisar Author e Livros
### Query
[block:code]
{
  "codes": [
    {
      "code": "{\n    authors{\n        nodes{\n            id,\n            name,\n            surname,\n            books{\n                id,\n                title,\n                price\n            }\n        }\n    }\n}",
      "language": "json"
    }
  ]
}
[/block]
Criar um novo Livro
### Mutation

[block:code]
{
  "codes": [
    {
      "code": "mutation{\n    createBook(book:{\n        id:7,\n        title : \"new book 4\",\n        authorId: 1,\n        price:12.5\n    }){id, title}\n\n}",
      "language": "text"
    }
  ]
}
[/block]

[block:image]
{
  "images": [
    {
      "image": [
        "https://files.readme.io/b62c559-PostmanCreateBook.jpg",
        "PostmanCreateBook.jpg",
        1487,
        716,
        "#2f3030"
      ]
    }
  ]
}
[/block]


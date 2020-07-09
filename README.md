# Poc-GraphQL-HotChocolate

### Referencia
https://www.blexin.com/en-US/Article/Blog/Creating-our-API-with-GraphQL-and-Hot-Chocolate-79
https://github.com/AARNOLD87/GraphQLWithHotChocolate

### Bibiotecas
1. Primeiro vamos criar uma web application, e adicionar as bibliotecas HotChocolate e HotChocolate.AspNetCore com o Nuget package manager. 
A biblioteca HotChocolate.AspNetCore.Playground será usando para fazer teste no browser da api.

### Configuração inicial
2. Configurar o GraphQL no startup da aplicação.
	2.1 ConfigureServices
[block:code]
{
  "codes": [
    {
      "code": "services.AddGraphQL(s => SchemaBuilder.New()\n                .AddServices(s)\n                .AddType<AuthorType>()\n                .AddType<BookType>()\n                .AddQueryType<Query>()\n                .Create());",
      "language": "csharp"
    }
  ]
}
[/block]
	2.2 Configure
[block:code]
{
  "codes": [
    {
      "code": "if (env.IsDevelopment())\n            {\n                app.UsePlayground();\n            }\n \n            app.UseGraphQL(\"/api\");\n",
      "language": "csharp"
    }
  ]
}
[/block]
### Model Author
3. Adicionar as models.
	3.1 Author
		
[block:code]
{
  "codes": [
    {
      "code": "public class Author\n\t\t{\n\t\t\tpublic int Id { get; set; }\n\t\t\tpublic string Name { get; set; }\n\t\t\tpublic string Surname { get; set; }\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
### Model Book		
3.2 Book
[block:code]
{
  "codes": [
    {
      "code": "public class Book\n\t\t{\n\t\t\tpublic int Id { get; set; }\n\t\t\tpublic int AuthorId { get; set; }\n\t\t\tpublic string Title { get; set; }\n\t\t\tpublic decimal Price { get; set; }\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
### Types
4. Definir as Types que serão expostos na api

	4.1 AuthorType
[block:code]
{
  "codes": [
    {
      "code": "public class AuthorType : ObjectType<Author>\n\t\t{\n\t\t\tprotected override void Configure(IObjectTypeDescriptor<Author> descriptor)\n\t\t\t{\n\t\t\t\tdescriptor.Field(a => a.Id).Type<IdType>();\n\t\t\t\tdescriptor.Field(a => a.Name).Type<StringType>();\n\t\t\t\tdescriptor.Field(a => a.Surname).Type<StringType>();\n\t\t\t}\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
		
		
	4.2 BookType
		
[block:code]
{
  "codes": [
    {
      "code": "public class BookType : ObjectType<Book>\n\t\t{\n\t\t\tprotected override void Configure(IObjectTypeDescriptor<Book> descriptor)\n\t\t\t{\n\t\t\t\tdescriptor.Field(b => b.Id).Type<IdType>();\n\t\t\t\tdescriptor.Field(b => b.Title).Type<StringType>();\n\t\t\t\tdescriptor.Field(b => b.Price).Type<DecimalType>();\n\t\t\t}\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
		
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
[block:code]
{
  "codes": [
    {
      "code": "public class Query\n\t\t{\n\t\t\tprivate readonly IAuthorService _authorService;\n\t\t\tpublic Query(IAuthorService authorService)\n\t\t\t{\n\t\t\t\t_authorService = authorService;\n\t\t\t}\n\t\t\t[UsePaging(SchemaType = typeof(AuthorType))]\n\t\t\t[UseFiltering]\n\t\t\tpublic IQueryable<Author> Authors => _authorService.GetAll();\n\t\t\t\n\t\t\t[UsePaging(SchemaType = typeof(BookType))]\n\t\t\t[UseFiltering]\n\t\t\tpublic IQueryable<Book> Books => _bookService.GetAll();\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
Adicionamos um novo campo ao esquema BookType com o descritor.Field e pedimos que ele o resolvesse pelo método GetAuthor do AuthorResolver		

### Resolvers
6. Resolver
	6.1 AuthorResolver
[block:code]
{
  "codes": [
    {
      "code": "public class AuthorResolver\n\t\t{\n\t\t    private readonly IAuthorService _authorService;\n\t\t \n\t\t    public AuthorResolver([Service]IAuthorService authorService)\n\t\t    {\n\t\t        _authorService = authorService;\n\t\t    }\n\t\t \n\t\t    public Author GetAuthor(Book book, IResolverContext ctx)\n\t\t    {\n\t\t        return _authorService.GetAll().Where(a => a.Id == book.AuthorId).FirstOrDefault();\n\t\t    }\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
### Mutation	
Mutation
Para concluir o CRUD com operações de criação e exclusão, precisamos implementar o que é chamado de mutação. É uma classe com métodos que indicam as operações possíveis. Ele deve ser gravado usando a seguinte instrução AddMutationTypez <Mutation> () que deve ser adicionada ao bloco de configuração do GraphQL em Startup.cs

7. Configuração
	7.1 startup
		7.2 ConfigureServices
[block:code]
{
  "codes": [
    {
      "code": "services.AddGraphQL(s => SchemaBuilder.New()\n\t\t\t.AddServices(s)\n\t\t\t.AddType<AuthorType>()\n\t\t\t.AddType<BookType>()\n\t\t\t.AddQueryType<Query>()\n\t\t\t.AddMutationType<Mutation>()\n\t\t\t.Create());\n",
      "language": "csharp"
    }
  ]
}
[/block]
O serviço precisará criar apenas uma nova instância do livro, adicioná-la à sua lista e retornar o livro criado.	
No playground, podemos testar o novo recurso iniciando a consulta.
		
8. Mutation	
[block:code]
{
  "codes": [
    {
      "code": "public class Mutation\n\t{\n\t\tprivate readonly IBookService _bookService;\n\t \n\t\tpublic Mutation(IBookService bookService)\n\t\t{\n\t\t\t_bookService = bookService;\n\t\t}\n\t \n\t\tpublic Book CreateBook(CreateBookInput inputBook)\n\t\t{\n\t\t\treturn _bookService.Create(inputBook);\n\t\t}\n\t}\n",
      "language": "csharp"
    }
  ]
}
[/block]
9. Input
	9.1 CreateBookInput
[block:code]
{
  "codes": [
    {
      "code": "public class CreateBookInput\n\t\t{\n\t\t\tpublic string Title { get; set; }\n\t\t\tpublic decimal Price { get; set; }\n\t\t\tpublic int AuthorId { get; set; }\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]
		
	
Vamos adicionar agora o recurso de remoção de livros alterando a Mutation existente acrescentando mais um método.
A classe DeleteBookInput possui apenas uma propriedade do tipo int que representa o ID do livro que você deseja excluir.
10. Mutation
[block:code]
{
  "codes": [
    {
      "code": "public class Mutation\n\t{\n\t\tprivate readonly IBookService _bookService;\n\t \n\t\t...\n\t\t\n\t\tpublic Book DeleteBook(DeleteBookInput inputBook)\n\t\t{\n\t\t\tvar bookToDelete = _books.Single(b => b.Id == inputBook.Id);\n\t\t\t_books.Remove(bookToDelete);\n\t\t\treturn bookToDelete;\n\t\t}\n\t}",
      "language": "csharp"
    }
  ]
}
[/block]
### Exceções
11. BookNotFoundException 
		
[block:code]
{
  "codes": [
    {
      "code": "public class BookNotFoundException : Exception\n\t\t{\n\t\t    public int BookId { get; internal set; }\n\t\t}",
      "language": "csharp"
    }
  ]
}
[/block]

[block:code]
{
  "codes": [
    {
      "code": "public Book Delete(DeleteBookInput inputBook)\n\t{\n\t\tvar bookToDelete = _books.FirstOrDefault(b => b.Id == inputBook.Id);\n\t\tif (bookToDelete == null)\n\t\tthrow new BookNotFoundException() { BookId = inputBook.Id };\n\t\t_books.Remove(bookToDelete);\n\t\treturn bookToDelete;\n\t}\n",
      "language": "csharp"
    }
  ]
}
[/block]
12. Alterando a método da mutation de deleção do objeto
	
		
13. IErrorFilter 
	
[block:code]
{
  "codes": [
    {
      "code": "public class BookNotFoundExceptionFilter : IErrorFilter\n\t{\n\t\tpublic IError OnError(IError error)\n\t\t{\n\t\t\t if (error.Exception is BookNotFoundException ex)\n\t\t\t\t return error.WithMessage($\"Book with id {ex.BookId} not found\");\n\t\t\t\t \n\t\t\t return error;\n\t\t}\n\t}",
      "language": "csharp"
    }
  ]
}
[/block]
14. Configuração do manipulador de erros no statup	
[block:code]
{
  "codes": [
    {
      "code": "public void ConfigureServices(IServiceCollection services){\n\t\t\n\t\t...\n\t\tservices.AddErrorFilter<BookNotFoundExceptionFilter>();\n\t\t\n\t}",
      "language": "csharp"
    }
  ]
}
[/block]
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


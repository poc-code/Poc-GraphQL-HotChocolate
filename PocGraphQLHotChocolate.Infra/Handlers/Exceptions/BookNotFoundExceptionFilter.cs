using HotChocolate;
using PocGraphQLHotChocolate.Infra.Model;

namespace PocGraphQLHotChocolate.Infra.Handlers.Exceptions
{
	public class BookNotFoundExceptionFilter : IErrorFilter
	{
		public IError OnError(IError error)
		{
			if (error.Exception is BookNotFoundException ex)
				return error.WithMessage($"Book with id {ex.BookId} not found");

			return error;
		}
	}
}

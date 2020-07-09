using System;

namespace PocGraphQLHotChocolate.Infra.Model
{
	public class BookNotFoundException : Exception
	{
		public int BookId { get; internal set; }
	}
}

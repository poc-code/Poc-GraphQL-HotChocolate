using System.Collections.Generic;

namespace PocGraphQLHotChocolate.Infra.Model
{
	public class Author
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}

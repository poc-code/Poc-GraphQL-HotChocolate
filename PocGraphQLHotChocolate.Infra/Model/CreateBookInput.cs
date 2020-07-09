using System;
using System.Collections.Generic;
using System.Text;

namespace PocGraphQLHotChocolate.Infra.Model
{
	public class CreateBookInput
	{
		public string Title { get; set; }
		public decimal Price { get; set; }
		public int AuthorId { get; set; }
	}
}

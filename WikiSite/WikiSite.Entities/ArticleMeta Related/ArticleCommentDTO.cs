using System;

namespace WikiSite.Entities
{
	public class ArticleCommentDTO
	{
		public Guid Id { get; set; }
		public Guid ArticleId { get; set; }
		public Guid AuthorId { get; set; }
		public string Text { get; set; }
		public DateTime DateOfCreation { get; set; }
	}
}
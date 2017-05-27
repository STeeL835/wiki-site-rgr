using System;

namespace WikiSite.Entities
{
	public class VersionVoteDTO
	{
		public Guid Id { get; set; }
		public Guid ArticleVersionId { get; set; }
		public Guid UserId { get; set; }
		public bool IsVoteFor { get; set; }
	}
}
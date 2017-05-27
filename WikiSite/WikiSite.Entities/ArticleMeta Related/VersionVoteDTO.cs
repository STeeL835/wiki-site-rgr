using System;

namespace WikiSite.Entities.ArticleMeta_Related
{
	public class VersionVoteDTO
	{
		public Guid Id { get; set; }
		public Guid ArticleVersionId { get; set; }
		public Guid UserId { get; set; }
		public bool Vote { get; set; }
	}
}
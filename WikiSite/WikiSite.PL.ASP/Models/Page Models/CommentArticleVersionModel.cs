using System;

namespace WikiSite.PL.ASP.Models
{
	public class CommentArticleVersionModel
	{
		public CVType CommentOrVersion { get; private set; }
		public CommentVM Comment { get; private set; }
		public ArticleVM ArticleVersion { get; private set; }
		public DateTime DateOfCreation { get; }

		public CommentArticleVersionModel(CommentVM comment)
		{
			Comment = comment;
			CommentOrVersion = CVType.Comment;
			DateOfCreation = comment.DateOfCreation;
		}

		public CommentArticleVersionModel(ArticleVM articleVersion)
		{
			ArticleVersion = articleVersion;
			CommentOrVersion = CVType.ArticleVersion;
			DateOfCreation = articleVersion.LastEditDate;
		}

		public enum CVType
		{
			Comment,
			ArticleVersion
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.Caretakers;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class ArticleCommentsBLO : IArticleCommentsBLL
	{
		private readonly IArticleCommentsDAL _articleCommentsDal;

		public ArticleCommentsBLO(IArticleCommentsDAL articleCommentsDal)
		{
			if (articleCommentsDal == null) throw new ArgumentNullException(nameof(articleCommentsDal), "DAL instance is null");
			_articleCommentsDal = articleCommentsDal;
		}

		/// <summary>
		/// Adds a comment for a certain article in db
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operation was successful</returns>
		public bool AddComment(ArticleCommentDTO comment)
		{
			CheckThrowDTO(comment);

			return _articleCommentsDal.AddComment(comment);
		}

		/// <summary>
		/// Removes comment from db
		/// </summary>
		/// <param name="commentId">Id of the comment</param>
		/// <returns>Whether operation was successful</returns>
		public bool RemoveComment(Guid commentId)
		{
			ErrorGuard.Check(commentId);

			return GetComment(commentId) != null && _articleCommentsDal.RemoveComment(commentId);
		}

		/// <summary>
		/// Updates comment text (not date)
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operaation was successful</returns>
		public bool UpdateComment(ArticleCommentDTO comment)
		{
			CheckThrowDTO(comment);

			/* If comment doesn't exist then what do you edit? */
			if (GetComment(comment.Id) == null) return false;

			/* Checking freshness of a comment if it's more than 2 hours old, then editing is bad*/
			if (DateTime.Now - comment.DateOfCreation > TimeSpan.FromHours(2)) return false;

			return _articleCommentsDal.UpdateComment(comment);
		}

		/// <summary>
		/// Returns comment by it's id
		/// </summary>
		/// <param name="commentId">id of the comment</param>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		public ArticleCommentDTO GetComment(Guid commentId)
		{
			ErrorGuard.Check(commentId);

			return _articleCommentsDal.GetComment(commentId);
		}

		/// <summary>
		/// Returns all comments that was written under a certain article
		/// </summary>
		/// <param name="articleId">id of the article</param>
		/// <returns>Comment DTOs</returns>
		public IEnumerable<ArticleCommentDTO> GetCommentsForArticle(Guid articleId)
		{
			ErrorGuard.Check(articleId);

			return _articleCommentsDal.GetCommentsForArticle(articleId).ToArray();
		}

		/// <summary>
		/// Returns all comments that was written by a certain user
		/// </summary>
		/// <param name="userId">id of the user</param>
		/// <returns>Comment DTOs</returns>
		public IEnumerable<ArticleCommentDTO> GetCommentsForUser(Guid userId)
		{
			ErrorGuard.Check(userId);

			return _articleCommentsDal.GetCommentsForUser(userId);
		}

		/// <summary>
		/// Returns the latest comment made on site
		/// </summary>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		public ArticleCommentDTO GetLatestComment()
		{
			return _articleCommentsDal.GetLatestComment();
		}

		public void CheckThrowDTO(ArticleCommentDTO dto)
		{
			ErrorGuard.Check(dto.Id, "DTO's id can't be empty");
			ErrorGuard.Check(dto.AuthorId, "Author's id can't be empty");
			ErrorGuard.Check(dto.ArticleId, "Article's id can't be empty");
			ErrorGuard.Check(dto.DateOfCreation);
			if (dto.DateOfCreation > DateTime.Now) throw new ArgumentException("Date can't be from future");
			ErrorGuard.Check(dto.Text, "Comment can't be null or empty");
		}
	}
}
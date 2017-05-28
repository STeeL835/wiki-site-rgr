using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
	public interface IArticleCommentsBLL
	{
		/// <summary>
		/// Adds a comment for a certain article in db
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operation was successful</returns>
		bool AddComment(ArticleCommentDTO comment);
		/// <summary>
		/// Removes comment from db
		/// </summary>
		/// <param name="commentId">Id of the comment</param>
		/// <returns>Whether operation was successful</returns>
		bool RemoveComment(Guid commentId);
		/// <summary>
		/// Updates comment text (not date)
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operaation was successful</returns>
		bool UpdateComment(ArticleCommentDTO comment);

		/// <summary>
		/// Returns comment by it's id
		/// </summary>
		/// <param name="commentId">id of the comment</param>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		ArticleCommentDTO GetComment(Guid commentId);
		/// <summary>
		/// Returns all comments that was written under a certain article
		/// </summary>
		/// <param name="articleId">id of the article</param>
		/// <returns>Comment DTOs</returns>
		IEnumerable<ArticleCommentDTO> GetCommentsForArticle(Guid articleId);
		/// <summary>
		/// Returns all comments that was written by a certain user
		/// </summary>
		/// <param name="userId">id of the user</param>
		/// <returns>Comment DTOs</returns>
		IEnumerable<ArticleCommentDTO> GetCommentsForUser(Guid userId);

		/// <summary>
		/// Returns the latest comment made on site
		/// </summary>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		ArticleCommentDTO GetLatestComment();
	}
}
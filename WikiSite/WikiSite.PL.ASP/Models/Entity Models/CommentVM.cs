using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
	public class CommentVM
	{
		#region Instance

		public Guid Id { get; set; }

		[Required(ErrorMessage = "Потерялся id статьи, попробуйте перезагрузить страницу")]
		public Guid ArticleId { get; set; }
		public Guid AuthorId { get; set; }

		[Required(ErrorMessage = "Текст комментария должен содержать текст")]
		[DataType(DataType.MultilineText)]
		[MaxLength(1500, ErrorMessage = "Длина комментария не может превышать 1500 символов")]
		public string Text { get; set; }
		public DateTime DateOfCreation { get; set; }

		/// <summary>
		/// Only for model binder
		/// </summary>
		public CommentVM()
		{
			Id = Guid.NewGuid();
			DateOfCreation = DateTime.Now;
		}

		public CommentVM(Guid articleId, Guid authorId, string text) : this()
		{
			ArticleId = articleId;
			AuthorId = authorId;
			Text = text;
		}

		public CommentVM(Guid id, Guid articleId, Guid authorId, string text, DateTime creationtime)
			: this(articleId, authorId, text)
		{
			Id = id;
			DateOfCreation = creationtime;
		}

		#endregion

		#region Static

		private static IArticleCommentsBLL _bll = Provider.ArticleCommentsBLO;

		/// <summary>
		/// Adds a comment for a certain article in db
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operation was successful</returns>
		public static bool AddComment(CommentVM comment)
		{
			return _bll.AddComment(Mapper.Map<ArticleCommentDTO>(comment));
		}

		/// <summary>
		/// Removes comment from db
		/// </summary>
		/// <param name="commentId">Id of the comment</param>
		/// <returns>Whether operation was successful</returns>
		public static bool RemoveComment(Guid commentId)
		{
			return _bll.RemoveComment(commentId);
		}

		/// <summary>
		/// Updates comment text (not date)
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operaation was successful</returns>
		public static bool UpdateComment(CommentVM comment)
		{
			return _bll.UpdateComment(Mapper.Map<ArticleCommentDTO>(comment));
		}

		/// <summary>
		/// Returns comment by it's id
		/// </summary>
		/// <param name="commentId">id of the comment</param>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		public static CommentVM GetComment(Guid commentId)
		{
			return Mapper.Map<CommentVM>(_bll.GetComment(commentId));
		}

		/// <summary>
		/// Returns all comments that was written under a certain article
		/// </summary>
		/// <param name="articleId">id of the article</param>
		/// <returns>Comment DTOs</returns>
		public static IEnumerable<CommentVM> GetCommentsForArticle(Guid articleId)
		{
			return Mapper.Map<IEnumerable<CommentVM>>(_bll.GetCommentsForArticle(articleId));
		}

		/// <summary>
		/// Returns all comments that was written by a certain user
		/// </summary>
		/// <param name="userId">id of the user</param>
		/// <returns>Comment DTOs</returns>
		public static IEnumerable<CommentVM> GetCommentsForUser(Guid userId)
		{
			return Mapper.Map<IEnumerable<CommentVM>>(_bll.GetCommentsForUser(userId));
		}

		/// <summary>
		/// Returns the latest comment made on site
		/// </summary>
		/// <returns>Comment DTO, null if doesn't exist</returns>
		public static CommentVM GetLatestComment()
		{
			return Mapper.Map<CommentVM>(_bll.GetLatestComment());
		}

		#endregion
	}
}
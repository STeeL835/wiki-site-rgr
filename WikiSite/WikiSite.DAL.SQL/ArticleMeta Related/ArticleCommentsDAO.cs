using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
	public class ArticleCommentsDAO : IArticleCommentsDAL
	{
		private static readonly string ConnectionString;

		static ArticleCommentsDAO()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
		}

		/// <summary>
		/// Adds a comment for a certain article in db
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operation was successful</returns>
		public bool AddComment(ArticleCommentDTO comment)
		{
			if (comment == null) throw new ArgumentNullException();

			int addedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("INSERT INTO [ArticleComments] (Id, Article_Id, Author_Id, Date_Of_Creation, Text) " +
				                            "VALUES(@id, @article_id, @author_id, @date_of_creation, @text)", connection);
				sqlCom.Parameters.AddWithValue("@id", comment.Id);
				sqlCom.Parameters.AddWithValue("@article_id", comment.ArticleId);
				sqlCom.Parameters.AddWithValue("@author_id", comment.AuthorId);
				sqlCom.Parameters.AddWithValue("@date_of_creation", comment.DateOfCreation);
				sqlCom.Parameters.AddWithValue("@text", comment.Text);
				connection.Open();

				addedRows = sqlCom.ExecuteNonQuery();
			}

			return addedRows == 1;
		}

		/// <summary>
		/// Removes comment from db
		/// </summary>
		/// <param name="commentId">Id of the comment</param>
		/// <returns>Whether operation was successful</returns>
		public bool RemoveComment(Guid commentId)
		{
			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("DELETE FROM [ArticleComments] WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", commentId);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Updates comment text (not date)
		/// </summary>
		/// <param name="comment">comment DTO</param>
		/// <returns>Whether operaation was successful</returns>
		public bool UpdateComment(ArticleCommentDTO comment)
		{
			if (comment == null) throw new ArgumentNullException();

			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("UPDATE [ArticleComments] " +
											"SET Text = @text " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", comment.Id);
				sqlCom.Parameters.AddWithValue("@text", comment.Text);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Returns comment by it's id
		/// </summary>
		/// <param name="commentId">id of the comment</param>
		/// <returns>Comment DTO</returns>
		public ArticleCommentDTO GetComment(Guid commentId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [ArticleComments] " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", commentId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new ArticleCommentDTO
					{
						Id = (Guid)reader["Id"],
						AuthorId = (Guid)reader["Author_Id"],
						DateOfCreation = (DateTime)reader["Date_Of_Creation"],
						ArticleId = (Guid)reader["Article_Id"],
						Text = (string)reader["Text"]
					};
				}
			}
			return null;
		}

		/// <summary>
		/// Returns all comments that was written under a certain article
		/// </summary>
		/// <param name="articleId">id of the article</param>
		/// <returns>Comment DTOs</returns>
		public IEnumerable<ArticleCommentDTO> GetCommentsForArticle(Guid articleId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [ArticleComments] " +
											"WHERE Article_Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", articleId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new ArticleCommentDTO
					{
						Id = (Guid)reader["Id"],
						AuthorId = (Guid)reader["Author_Id"],
						DateOfCreation = (DateTime)reader["Date_Of_Creation"],
						ArticleId = (Guid)reader["Article_Id"],
						Text = (string)reader["Text"]
					};
				}
			}
		}

		/// <summary>
		/// Returns all comments that was written by a certain user
		/// </summary>
		/// <param name="userId">id of the user</param>
		/// <returns>Comment DTOs</returns>
		public IEnumerable<ArticleCommentDTO> GetCommentsForUser(Guid userId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [ArticleComments] " +
											"WHERE Article_Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", userId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new ArticleCommentDTO
					{
						Id = (Guid)reader["Id"],
						AuthorId = (Guid)reader["Author_Id"],
						DateOfCreation = (DateTime)reader["Date_Of_Creation"],
						ArticleId = (Guid)reader["Article_Id"],
						Text = (string)reader["Text"]
					};
				}
			}
		}

		/// <summary>
		/// Returns the latest comment made on site
		/// </summary>
		/// <returns>Comment DTO</returns>
		public ArticleCommentDTO GetLatestComment()
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT TOP 1 * FROM [ArticleComments] ORDER BY Date_Of_Creation DESC", connection);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new ArticleCommentDTO
					{
						Id = (Guid)reader["Id"],
						AuthorId = (Guid)reader["Author_Id"],
						DateOfCreation = (DateTime)reader["Date_Of_Creation"],
						ArticleId = (Guid)reader["Article_Id"],
						Text = (string)reader["Text"]
					};
				}
			}
			return null;
		}
	}
}
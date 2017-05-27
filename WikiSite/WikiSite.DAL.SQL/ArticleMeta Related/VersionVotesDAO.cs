using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
	public class VersionVotesDAO : IVersionVotesDAL
	{
		private static readonly string ConnectionString;

		static VersionVotesDAO()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
		}

		/// <summary>
		/// Adds a vote of user in db
		/// </summary>
		/// <param name="vote">vote DTO</param>
		/// <returns>Whether operation was successful</returns>
		public bool AddVote(VersionVoteDTO vote)
		{
			if (vote == null) throw new ArgumentNullException();

			int addedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("INSERT INTO [VersionVotes] (Id, ArticleVersion_Id, User_Id, Vote) " +
											"VALUES(@id, @version_id, @user_id, @vote)", connection);
				sqlCom.Parameters.AddWithValue("@id", vote.Id);
				sqlCom.Parameters.AddWithValue("@version_id", vote.ArticleVersionId);
				sqlCom.Parameters.AddWithValue("@user_id", vote.UserId);
				sqlCom.Parameters.AddWithValue("@vote", vote.IsVoteFor);
				connection.Open();

				addedRows = sqlCom.ExecuteNonQuery();
			}

			return addedRows == 1;
		}

		/// <summary>
		/// Removes user's vote from db
		/// </summary>
		/// <param name="voteId">Id of vote dto</param>
		/// <returns>Whether operation was successful</returns>
		public bool RemoveVote(Guid voteId)
		{
			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("DELETE FROM [VersionVotes] WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", voteId);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Changes vote of user
		/// </summary>
		/// <param name="vote">Id of vote dto</param>
		/// <returns>Whether operation was successful</returns>
		public bool UpdateVote(VersionVoteDTO vote)
		{
			if (vote == null) throw new ArgumentNullException();

			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("UPDATE [VersionVotes] " +
											"SET ArticleVersion_Id = @version_id, User_Id = @user_id, Vote = @vote " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", vote.Id);
				sqlCom.Parameters.AddWithValue("@version_id", vote.ArticleVersionId);
				sqlCom.Parameters.AddWithValue("@user_id", vote.UserId);
				sqlCom.Parameters.AddWithValue("@vote", vote.IsVoteFor);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Gets vote from db by it's id
		/// </summary>
		/// <param name="voteId">Id of vote DTO</param>
		/// <returns>Vote DTO</returns>
		public VersionVoteDTO GetVote(Guid voteId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [VersionVotes] " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", voteId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new VersionVoteDTO
					{
						Id = (Guid)reader["Id"],
						UserId = (Guid)reader["User_Id"],
						ArticleVersionId = (Guid)reader["ArticleVersion_Id"],
						IsVoteFor = (bool)reader["Vote"]
					};
				}
			}
			return null;
		}

		/// <summary>
		/// Gets all votes made by user from db
		/// </summary>
		/// <param name="userId">id of a user</param>
		/// <returns>Votes DTO</returns>
		public IEnumerable<VersionVoteDTO> GetVotesOfUser(Guid userId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [VersionVotes] " +
											"WHERE User_Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", userId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new VersionVoteDTO
					{
						Id = (Guid)reader["Id"],
						UserId = (Guid)reader["User_Id"],
						ArticleVersionId = (Guid)reader["ArticleVersion_Id"],
						IsVoteFor = (bool)reader["Vote"]
					};
				}
			}
		}

		/// <summary>
		/// Gets all votes made about certain article
		/// </summary>
		/// <param name="articleVersionId">Id of an article</param>
		/// <returns>Votes DTO</returns>
		public IEnumerable<VersionVoteDTO> GetVotesForVersion(Guid articleVersionId)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [VersionVotes] " +
											"WHERE ArticleVersion_Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", articleVersionId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new VersionVoteDTO
					{
						Id = (Guid)reader["Id"],
						UserId = (Guid)reader["User_Id"],
						ArticleVersionId = (Guid)reader["ArticleVersion_Id"],
						IsVoteFor = (bool)reader["Vote"]
					};
				}
			}
		}
	}
}
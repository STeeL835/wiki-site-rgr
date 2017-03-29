using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
    public class UsersDAO : IUsersDAL
    {
		private static readonly string ConnectionString;

		static UsersDAO()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
		}

		/// <summary>
		/// Adds user to a database
		/// </summary>
		/// <remarks>
		/// This method doesn't count SmallID field since it 
		/// managed by an SQL Database.
		/// </remarks>
		/// <param name="user">User DTO</param>
		public bool AddUser(UserDTO user)
	    {
			if (user == null) throw new ArgumentNullException();

			int addedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("INSERT INTO [Users] (Id, Nickname, Role_Id, Credentials_Id, About) " +
											"VALUES(@id, @nickname, @role_id, @credentials_id, @about)", connection);
				sqlCom.Parameters.AddWithValue("@id", user.Id);
				sqlCom.Parameters.AddWithValue("@nickname", user.Nickname);
				sqlCom.Parameters.AddWithValue("@role_id", user.RoleId == Guid.Empty ? (object)"DEFAULT" : user.RoleId);
				sqlCom.Parameters.AddWithValue("@credentials_id", user.CredentialsId);
				sqlCom.Parameters.AddWithValue("@about", user.About);
				connection.Open();

				addedRows = sqlCom.ExecuteNonQuery();
			}

			return addedRows == 1;
		}

	    /// <summary>
	    /// Updates user info in a database
	    /// </summary>
	    /// <remarks>
	    /// This method doesn't count SmallID field since it 
	    /// managed by an SQL Database.
	    /// </remarks>
	    /// <param name="user">User DTO with the same ID and new data</param>
	    public bool UpdateUser(UserDTO user)
	    {
			if (user == null) throw new ArgumentNullException();

			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("UPDATE [Users] " +
											"SET Nickname = @nickname, Role_Id = @role_id, About = @about" +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", user.Id);
				sqlCom.Parameters.AddWithValue("@nickname", user.Nickname);
				sqlCom.Parameters.AddWithValue("@role_id", user.RoleId);
				sqlCom.Parameters.AddWithValue("@about", user.About);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

	    /// <summary>
	    /// Removes user from a database.
	    /// </summary>
	    /// <param name="userId">GUID of user to delete</param>
	    public bool RemoveUser(Guid userId)
	    {
		    var user = GetUser(userId);

			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("DELETE FROM [Credentials] WHERE Id = @cred_id", connection);
				sqlCom.Parameters.AddWithValue("@cred_id", user.CredentialsId);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

	    /// <summary>
	    /// Gets all users from database
	    /// </summary>
	    /// <returns>Users' DTOs</returns>
	    public IEnumerable<UserDTO> GetUsers()
	    {
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Users]", connection);
				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new UserDTO
					{
						Id = (Guid)reader["Id"],
						SmallId = (int)reader["Small_Id"],
						CredentialsId = (Guid)reader["Credentials_Id"],
						Nickname = (string)reader["Nickname"],
						RoleId = (Guid)reader["Role_Id"],
						About = (string)reader["About"]
					};
				}
			}
		}

	    /// <summary>
	    /// Gets all users from database with a certain role
	    /// </summary>
	    /// <param name="roleId">GUID of a role</param>
	    /// <returns>Users' DTOs</returns>
	    public IEnumerable<UserDTO> GetUsers(Guid roleId)
	    {
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Users] WHERE [Users].[Role_Id] = @role_id", connection);
				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new UserDTO
					{
						Id = (Guid)reader["Id"],
						SmallId = (int)reader["Small_Id"],
						CredentialsId = (Guid)reader["Credentials_Id"],
						Nickname = (string)reader["Nickname"],
						RoleId = (Guid)reader["Role_Id"],
						About = (string)reader["About"]
					};
				}
			}
		}

	    /// <summary>
	    /// Gets a certain user from a database
	    /// </summary>
	    /// <param name="userId">GUID of user to get</param>
	    /// <returns>DTO of a user</returns>
	    public UserDTO GetUser(Guid userId)
	    {
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Users] " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", userId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new UserDTO
					{
						Id = (Guid)reader["Id"],
						SmallId = (int)reader["Small_Id"],
						CredentialsId = (Guid)reader["Credentials_Id"],
						Nickname = (string)reader["Nickname"],
						RoleId = (Guid)reader["Role_Id"],
						About = (string)reader["About"]
					};
				}
			}
			throw new EntryNotFoundException($"Product with id {userId} was not found");
		}

	    /// <summary>
	    /// Gets a certain user from a database
	    /// </summary>
	    /// <param name="userShortId">Incremental ID (number) of user to get</param>
	    /// <returns>DTO of a user</returns>
	    public UserDTO GetUser(int userShortId)
	    {
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Users] " +
											"WHERE Short_Id = @short_id", connection);
				sqlCom.Parameters.AddWithValue("@short_id", userShortId);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new UserDTO
					{
						Id = (Guid)reader["Id"],
						SmallId = (int)reader["Small_Id"],
						CredentialsId = (Guid)reader["Credentials_Id"],
						Nickname = (string)reader["Nickname"],
						RoleId = (Guid)reader["Role_Id"],
						About = (string)reader["About"]
					};
				}
			}
			throw new EntryNotFoundException($"Product with id {userShortId} was not found");
		}

	    /// <summary>
	    /// Searches for the user in database by it's nickname
	    /// </summary>
	    /// <param name="searchInput">Search string</param>
	    /// <returns>Collection of users whose nickname matches the criteria</returns>
	    public IEnumerable<UserDTO> SearchUsers(string searchInput)
	    {
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Users] " +
											"WHERE Nickname LIKE '%@short_id%'", connection);
				sqlCom.Parameters.AddWithValue("@short_id", searchInput);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new UserDTO
					{
						Id = (Guid)reader["Id"],
						SmallId = (int)reader["Small_Id"],
						CredentialsId = (Guid)reader["Credentials_Id"],
						Nickname = (string)reader["Nickname"],
						RoleId = (Guid)reader["Role_Id"],
						About = (string)reader["About"]
					};
				}
			}
		}
    }
}

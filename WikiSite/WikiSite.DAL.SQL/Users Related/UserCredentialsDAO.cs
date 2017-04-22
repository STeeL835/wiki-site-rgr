using System;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
	public class UserCredentialsDAO : IUserCredentialsDAL
	{
		private static readonly string ConnectionString;

		static UserCredentialsDAO()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
		}

		/// <summary>
		/// Gets login and password hash
		/// </summary>
		/// <param name="id">credentials id</param>
		/// <exception cref="EntryNotFoundException">Credentials do not exist</exception>
		/// <returns>Credentials DTO</returns>
		public UserCredentialsOutDTO GetCredentials(Guid id)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Credentials] " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", id);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new UserCredentialsOutDTO
					{
						Id = (Guid)reader["Id"],
						Login = (string)reader["Login"],
						PasswordHash = (byte[])reader["Password_Hash"],
					};
				}
			}
			throw new EntryNotFoundException($"Credentials with id {id} was not found");
		}

		/// <summary>
		/// Searches credentials info in db, and if found, returns user
		/// </summary>
		/// <param name="credentials">Credentials to search for</param>
		/// <returns>null if credentials aren't valid, UserDTO otherwise</returns>
		public UserDTO CheckCredentials(UserCredentialsOutDTO credentials)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT [Users].* " +
				                            "FROM [Users] " +
				                            "INNER JOIN [Credentials] ON [Users].[Credentials_Id] = [Credentials].[Id] " +
											"WHERE [Credentials].[Login] = @login AND [Credentials].[Password_Hash] = @hash", connection);
				sqlCom.Parameters.AddWithValue("@login", credentials.Login);
				sqlCom.Parameters.AddWithValue("@hash", credentials.PasswordHash);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				return !reader.Read() ? null : new UserDTO
				{
					Id = (Guid)reader["Id"],
					ShortId = (int)reader["Short_Id"],
					CredentialsId = (Guid)reader["Credentials_Id"],
					Nickname = (string)reader["Nickname"],
					RoleId = (Guid)reader["Role_Id"],
					About = reader["About"] as string
				};
			}
		}

		/// <summary>
		/// Creates credentials in db
		/// </summary>
		/// <param name="credentials">Sign in info</param>
		/// <returns>Whether creation is successful</returns>
		public bool AddCredentials(UserCredentialsOutDTO credentials)
		{
			if (credentials == null) throw new ArgumentNullException();

			int addedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("INSERT INTO [Credentials] (Id, Login, Password_Hash) " +
											"VALUES(@id, @login, @hash)", connection);
				sqlCom.Parameters.AddWithValue("@id", credentials.Id);
				sqlCom.Parameters.AddWithValue("@login", credentials.Login);
				sqlCom.Parameters.AddWithValue("@hash", credentials.PasswordHash);
				connection.Open();

				addedRows = sqlCom.ExecuteNonQuery();
			}

			return addedRows == 1;
		}

		/// <summary>
		/// Removes user credentials from DB. 
		/// </summary>
		/// <remarks>
		/// Be careful, it also will delete user that belongs to them as long 
		/// as it's useless if you can't log in
		/// </remarks>
		/// <param name="id">credentials id</param>
		/// <returns>Whether credentials was deleted</returns>
		public bool RemoveCredentials(Guid id)
		{
			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("DELETE FROM [Credentials] WHERE Id = @cred_id", connection);
				sqlCom.Parameters.AddWithValue("@cred_id", id);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Updates sign in info
		/// </summary>
		/// <param name="credentials">credentials id</param>
		/// <returns>Whether there is credentials with this id, and it was updated</returns>
		public bool UpdateCredentials(UserCredentialsOutDTO credentials)
		{
			if (credentials == null) throw new ArgumentNullException();

			int affectedRows;
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("UPDATE [Credentials] " +
											"SET Login = @login, Password_Hash = @hash " +
											"WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", credentials.Id);
				sqlCom.Parameters.AddWithValue("@login", credentials.Login);
				sqlCom.Parameters.AddWithValue("@hash", credentials.PasswordHash);
				connection.Open();

				affectedRows = sqlCom.ExecuteNonQuery();
			}

			return affectedRows == 1;
		}

		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		public bool IsLoginExist(string login)
		{
			if (login == null) throw new ArgumentNullException(nameof(login), "Login string parameter is null");

			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Credentials] " +
											"WHERE Login = @login", connection);
				sqlCom.Parameters.AddWithValue("@login", login);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				return reader.Read();
			}
		}
	}
}
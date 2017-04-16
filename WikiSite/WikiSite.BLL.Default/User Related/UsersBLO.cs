using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class UsersBLO : IUsersBLL
	{
		private IUsersDAL _usersDAL;
		private IUserCredentialsDAL _credentialsDAL;

		public UsersBLO(IUsersDAL usersDAL, IUserCredentialsDAL credentialsDAL)
		{
			if (usersDAL == null) throw new ArgumentNullException(nameof(usersDAL), "Users DAL instance is null");
			if (credentialsDAL == null) throw new ArgumentNullException(nameof(usersDAL), "Credentials DAL instance is null");
			
			_usersDAL = usersDAL;
			_credentialsDAL = credentialsDAL;
		}

		/// <summary>
		/// Adds user to a database
		/// </summary>
		/// <remarks>
		/// This method doesn't count SmallID field since it 
		/// managed by an SQL Database.
		/// </remarks>
		/// <param name="user">User DTO</param>
		/// <param name="credentials">Credentials DTO</param>
		public bool AddUser(UserDTO user, UserCredentialsDTO credentials)
		{
			CheckThrowDTO(user);
			CheckThrowDTO(credentials);
			if (user.CredentialsId != credentials.Id)
				throw new ArgumentException("user's credentials id and actual credentials id doesn't match");

			return !IsLoginExist(credentials.Login) && 
			       _credentialsDAL.AddCredentials(credentials) && _usersDAL.AddUser(user);
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
			CheckThrowDTO(user);

			return _usersDAL.UpdateUser(user);
		}

		/// <summary>
		/// Updates credentials info in database
		/// </summary>
		/// <param name="updatedCredentials">Credentials DTO with the same ID and new data</param>
		/// <returns>Whether the operation was successful</returns>
		public bool UpdateUserCredentials(UserCredentialsDTO updatedCredentials)
		{
			CheckThrowDTO(updatedCredentials);
			return _credentialsDAL.UpdateCredentials(updatedCredentials);
		}

		/// <summary>
		/// Removes user from a database.
		/// </summary>
		/// <param name="userId">GUID of user to delete</param>
		public bool RemoveUser(Guid userId)
		{
			if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "Id is empty");

			return _credentialsDAL.RemoveCredentials(GetUser(userId).CredentialsId) && 
			       _usersDAL.RemoveUser(userId);
		}

		/// <summary>
		/// Gets all users from database
		/// </summary>
		/// <returns>Users' DTOs</returns>
		public IEnumerable<UserDTO> GetUsers()
		{
			return _usersDAL.GetUsers().ToArray(); 
		}

		/// <summary>
		/// Gets all users from database with a certain role
		/// </summary>
		/// <param name="roleId">GUID of a role</param>
		/// <returns>Users' DTOs</returns>
		public IEnumerable<UserDTO> GetUsers(Guid roleId)
		{
			if (roleId == Guid.Empty) throw new ArgumentNullException(nameof(roleId), "Id is empty");

			return _usersDAL.GetUsers(roleId).ToArray();
		}

		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userId">GUID of user to get</param>
		/// <returns>DTO of a user</returns>
		public UserDTO GetUser(Guid userId)
		{
			if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "Id is empty");

			return _usersDAL.GetUser(userId);
		}

		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userShortId">Incremental ID (number) of user to get</param>
		/// <returns>DTO of a user, null if there's no such a user</returns>
		public UserDTO GetUser(int userShortId)
		{
			if (userShortId <= 0) throw new ArgumentException("Short user id must be greater than 0");

			return _usersDAL.GetUser(userShortId);
		}

		/// <summary>
		/// Checks if a user with these credentials exist in a database
		/// and returns it.
		/// </summary>
		/// <param name="credentials">Credentials to check for in a database</param>
		/// <returns>DTO of a user, null if there's no such a user</returns>
		public UserDTO GetUser(UserCredentialsDTO credentials)
		{
			if (credentials == null) throw new ArgumentNullException(nameof(credentials), "Credentials DTO is null");

			if (string.IsNullOrWhiteSpace(credentials.Login)) throw new ArgumentException("Credentials DTO doesn't contain login or it's empty");
			if (credentials.PasswordHash == null || credentials.PasswordHash.Length == 0) throw new ArgumentException("Credentials DTO doesn't contain password hash or it's empty");

			return _credentialsDAL.CheckCredentials(credentials);
		}

		/// <summary>
		/// Searches for the user in database by it's nickname
		/// </summary>
		/// <param name="searchInput">Search string</param>
		/// <returns>Collection of users whose nickname matches the criteria</returns>
		public IEnumerable<UserDTO> SearchUsers(string searchInput)
		{
			if (string.IsNullOrWhiteSpace(searchInput) || searchInput.Length < 5)
				throw new ArgumentNullException(nameof(searchInput), "Search query should be more than 4 charachter long");

			return _usersDAL.SearchUsers(searchInput).ToArray();
		}

		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		public bool IsLoginExist(string login)
		{
			if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login string is null or empty");

			return _credentialsDAL.IsLoginExist(login);
		}

		private void CheckThrowDTO(UserDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "User DTO is null");

			if (dto.CredentialsId == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain credentials ID");
			if (dto.Id == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain ID");
			if (string.IsNullOrWhiteSpace(dto.Nickname)) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain a nickname");
		}
		private void CheckThrowDTO(UserCredentialsDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "Credentials DTO is null");
			
			if (dto.Id == Guid.Empty) throw new ArgumentNullException(nameof(dto), "Credentials DTO doesn't contain ID");
			if (string.IsNullOrWhiteSpace(dto.Login)) throw new ArgumentException("Credentials DTO doesn't contain login or it's empty");
			if (dto.PasswordHash == null || dto.PasswordHash.Length == 0) throw new ArgumentException("Credentials DTO doesn't contain password hash or it's empty");
		}
	}
}

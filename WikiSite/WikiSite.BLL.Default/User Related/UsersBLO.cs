using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class UsersBLO : IUsersBLL
	{
		private SHA512 HashFunction { get; } = SHA512.Create();

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
		public bool AddUser(UserDTO user, UserCredentialsInDTO credentials)
		{
			CheckThrowDTO(user);
			CheckThrowDTO(credentials);

			if (user.CredentialsId != credentials.Id)
				throw new ArgumentException("user's credentials id and actual credentials id doesn't match");

			return !IsLoginExist(credentials.Login) && 
			       _credentialsDAL.AddCredentials(Out(credentials)) && _usersDAL.AddUser(user);
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
		public bool UpdateUserCredentials(UserCredentialsInDTO updatedCredentials)
		{
			CheckThrowDTO(updatedCredentials);
			return _credentialsDAL.UpdateCredentials(Out(updatedCredentials));
		}

		/// <summary>
		/// Removes user from a database.
		/// </summary>
		/// <param name="userId">GUID of user to delete</param>
		public bool RemoveUser(Guid userId)
		{
			if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "Id is empty");
			var credId = GetUser(userId).CredentialsId;
			return _usersDAL.RemoveUser(userId) && _credentialsDAL.RemoveCredentials(credId);
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
		/// <remarks>
		/// There guid is not necessary, empty guid can be passed.
		/// </remarks>
		/// <param name="credentials">Credentials to check for in a database</param>
		/// <returns>DTO of a user, null if there's no such a user</returns>
		public UserDTO GetUser(UserCredentialsInDTO credentials)
		{
			if (credentials == null) throw new ArgumentNullException(nameof(credentials), "Credentials DTO is null");

			if (string.IsNullOrWhiteSpace(credentials.Login))
				throw new ArgumentException("Credentials DTO doesn't contain login or it's empty");
			if (string.IsNullOrWhiteSpace(credentials.Password) || credentials.Password.Length < 8)
				throw new ArgumentException("Credentials DTO doesn't contain password or it's empty");

			return _credentialsDAL.CheckCredentials(Out(credentials));
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
		/// Returns login for a certain user
		/// </summary>
		/// <param name="userId">id of a user</param>
		/// <returns>login string</returns>
		public string GetLogin(Guid userId)
		{
			return _credentialsDAL.GetCredentials(GetUser(userId).CredentialsId).Login;
		}

		/// <summary>
		/// Checks passwords (passed in and real one) if they match
		/// </summary>
		/// <remarks>
		/// Id is not checked.
		/// </remarks>
		/// <param name="credentials">login-pass pair</param>
		/// <returns>Whether the password is match the original</returns>
		public bool IsPasswordMatch(UserCredentialsInDTO credentials)
		{
			return GetUser(credentials) != null;
		}

		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		public bool IsLoginExist(string login)
		{
			if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login string is null or empty");

			return _credentialsDAL.IsLoginExist(login.ToLowerInvariant());
		}

		private void CheckThrowDTO(UserDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "User DTO is null");

			if (dto.CredentialsId == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain credentials ID");
			if (dto.Id == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain ID");
			if (string.IsNullOrWhiteSpace(dto.Nickname)) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain a nickname");
		}
		private void CheckThrowDTO(UserCredentialsInDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "Credentials DTO is null");
			
			if (dto.Id == Guid.Empty) throw new ArgumentNullException(nameof(dto), "Credentials DTO doesn't contain ID");
			if (string.IsNullOrWhiteSpace(dto.Login)) throw new ArgumentException("Credentials DTO doesn't contain login or it's empty");
			if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8) throw new ArgumentException("Credentials DTO doesn't contain password or it's empty");
		}
		/// <summary>
		/// Decapitalizes login
		/// </summary>
		/// <param name="dto">Credentials DTO</param>
		private void LowerifyLogin(UserCredentialsInDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "Credentials DTO is null");

			dto.Login = dto.Login.ToLowerInvariant();
		}
		/// <summary>
		/// Hashes the password with SHA2-512 algorythm
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		private byte[] GetHash(string password)
		{
			if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
				throw new ArgumentException("password is null, less than 8 characters long or it's empty");
			return HashFunction.ComputeHash(Encoding.UTF8.GetBytes(password));
		}
		/// <summary>
		/// Casts IN dto to OUT dto (password->hash)
		/// </summary>
		/// <param name="dtoFromPL"></param>
		/// <returns></returns>
		private UserCredentialsOutDTO Out(UserCredentialsInDTO dtoFromPL)
		{
			if (dtoFromPL == null) throw new ArgumentNullException(nameof(dtoFromPL), "Credentials DTO is null");
			LowerifyLogin(dtoFromPL);
			return new UserCredentialsOutDTO {Id = dtoFromPL.Id, Login = dtoFromPL.Login, PasswordHash = GetHash(dtoFromPL.Password)};
		}
	}
}

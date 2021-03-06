﻿using System;
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

		private readonly IUsersDAL _usersDAL;
		private readonly IUserCredentialsDAL _credentialsDAL;

		public UsersBLO(IUsersDAL usersDAL, IUserCredentialsDAL credentialsDAL)
		{
			if (usersDAL == null) throw new ArgumentNullException(nameof(usersDAL), "Users DAL instance is null");
			if (credentialsDAL == null) throw new ArgumentNullException(nameof(credentialsDAL), "Credentials DAL instance is null");
			
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

			var success = !IsLoginExist(credentials.Login);
			if (success) // if login doesn't exist
			{
				var credSuccess = _credentialsDAL.AddCredentials(InDB(credentials));
				var userSuccess = _usersDAL.AddUser(user);
				if (credSuccess && !userSuccess)
				{
					_credentialsDAL.RemoveCredentials(credentials.Id); //clean up if user wasn't added
					throw new OperationCanceledException("User was not added");
				}
				success &= credSuccess &= userSuccess;
			}

			return success;
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
			return _credentialsDAL.UpdateCredentials(InDB(updatedCredentials));
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
		public UserDTO GetUser(UserCredentialsDTO credentials)
		{
			CheckThrowDTOAuth(credentials);

			return _credentialsDAL.CheckCredentials(InDB(credentials));
		}

		/// <summary>
		/// Searches for the user in database by it's nickname
		/// </summary>
		/// <param name="searchInput">Search string</param>
		/// <returns>Collection of users whose nickname matches the criteria</returns>
		public IEnumerable<UserDTO> SearchUsers(string searchInput)
		{
			if (string.IsNullOrWhiteSpace(searchInput) || searchInput.Length < 3)
				throw new ArgumentNullException(nameof(searchInput), "Search query should be more than 2 character long");

			/* Wildcards support */
			searchInput = searchInput.Replace("[", "[[]").Replace("%", "[%]").Replace("‌​_", "[_]"); // escaping sql wildcards
			searchInput = searchInput.Replace("?", "_").Replace("*", "%"); //converting

			var found = new List<UserDTO>();

			/* Nickname */
			found.AddRange(_usersDAL.SearchUsers(searchInput));

			/* ID */
			var shortId = 0;
			if (int.TryParse(searchInput, out shortId))
			{
				try {
					found.Add(GetUser(shortId));
				}
				catch (EntryNotFoundException) { } // but throws other exceptions
			}
			/* GUID */
			var id = Guid.Empty;
			if (Guid.TryParse(searchInput, out id))
			{
				try {
					found.Add(GetUser(id));
				}
				catch (EntryNotFoundException) { } // but throws other exceptions
			}

			return found;
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
		/// Returns email for a certain user
		/// </summary>
		/// <param name="userId">id of a user</param>
		/// <returns>email string</returns>
		public string GetEmail(Guid userId)
		{
			return _credentialsDAL.GetCredentials(GetUser(userId).CredentialsId).Email;
		}

		/// <summary>
		/// Checks passwords (passed in and real one) if they match
		/// </summary>
		/// <remarks>
		/// Id is not checked.
		/// </remarks>
		/// <param name="credentials">login-pass pair</param>
		/// <returns>Whether the password is match the original</returns>
		public bool IsPasswordMatch(UserCredentialsDTO credentials)
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

		/// <summary>
		/// Checks for email in db. Returns whether email does exist or not
		/// </summary>
		/// <param name="email">email string</param>
		/// <returns>Whether email does exist or not</returns>
		public bool IsEmailExist(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email string is null or empty");

			return _credentialsDAL.IsEmailExist(email.ToLowerInvariant());
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
			if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Credentials DTO doesn't contain email or it's empty");
			if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8) throw new ArgumentException("Credentials DTO doesn't contain password or it's empty");
		}
		/// <summary>
		/// Checks if dto is valid for authentification. If not, throws exceptions
		/// </summary>
		/// <remarks>
		/// It doesn't check id, but if email and login are BOTH missing, throws
		/// </remarks>
		/// <param name="dto">DTO from PL layer</param>
		private void CheckThrowDTOAuth(UserCredentialsDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "Credentials DTO is null");
 
			if (string.IsNullOrWhiteSpace(dto.Login) && string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Credentials DTO doesn't contain login or email");
			if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8) throw new ArgumentException("Credentials DTO doesn't contain password or it's empty");
		}
		/// <summary>
		/// Makes login and email lowercase
		/// </summary>
		/// <param name="dto">Credentials DTO</param>
		private void Lowerify(UserCredentialsDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "Credentials DTO is null");

			dto.Login = dto.Login?.ToLowerInvariant();
			dto.Email = dto.Email?.ToLowerInvariant();
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
		/// Casts dto from PL to db dto (password->hash)
		/// </summary>
		/// <param name="dtoFromPL"></param>
		/// <returns></returns>
		private UserCredentialsDBDTO InDB(UserCredentialsDTO dtoFromPL)
		{
			if (dtoFromPL == null) throw new ArgumentNullException(nameof(dtoFromPL), "Credentials DTO is null");//TODO: use ErrorGuard
			Lowerify(dtoFromPL);
			return new UserCredentialsDBDTO //TODO: map this with Mapper
				{
					Id = dtoFromPL.Id,
					Login = dtoFromPL.Login,
					Email = dtoFromPL.Email,
					PasswordHash = GetHash(dtoFromPL.Password)
				};
		}
	}
}

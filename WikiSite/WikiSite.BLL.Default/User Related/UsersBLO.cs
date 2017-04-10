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
		private IUsersDAL _dal;

		public UsersBLO(IUsersDAL dal)
		{
			if (dal == null) throw new ArgumentNullException(nameof(dal), "DAL instance is null");
			_dal = dal;
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
			CheckThrowDTO(user);

			return _dal.AddUser(user);
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

			return _dal.UpdateUser(user);
		}

		/// <summary>
		/// Removes user from a database.
		/// </summary>
		/// <param name="userId">GUID of user to delete</param>
		public bool RemoveUser(Guid userId)
		{
			if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "Id is empty");

			return _dal.RemoveUser(userId);
		}

		/// <summary>
		/// Gets all users from database
		/// </summary>
		/// <returns>Users' DTOs</returns>
		public IEnumerable<UserDTO> GetUsers()
		{
			return _dal.GetUsers().ToArray(); 
		}

		/// <summary>
		/// Gets all users from database with a certain role
		/// </summary>
		/// <param name="roleId">GUID of a role</param>
		/// <returns>Users' DTOs</returns>
		public IEnumerable<UserDTO> GetUsers(Guid roleId)
		{
			if (roleId == Guid.Empty) throw new ArgumentNullException(nameof(roleId), "Id is empty");

			return _dal.GetUsers(roleId).ToArray();
		}

		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userId">GUID of user to get</param>
		/// <returns>DTO of a user</returns>
		public UserDTO GetUser(Guid userId)
		{
			if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId), "Id is empty");

			return _dal.GetUser(userId);
		}

		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userShortId">Incremental ID (number) of user to get</param>
		/// <returns>DTO of a user</returns>
		public UserDTO GetUser(int userShortId)
		{
			if (userShortId <= 0) throw new ArgumentException("Short user id must be greater than 0");

			return _dal.GetUser(userShortId);
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

			return _dal.SearchUsers(searchInput).ToArray();
		}

		public void CheckThrowDTO(UserDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "DTO mustn't be null");

			if (dto.CredentialsId == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain credentials ID");
			if (dto.Id == Guid.Empty) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain ID");
			if (string.IsNullOrWhiteSpace(dto.Nickname)) throw new ArgumentNullException(nameof(dto), "User DTO doesn't contain a nickname");
		}
	}
}

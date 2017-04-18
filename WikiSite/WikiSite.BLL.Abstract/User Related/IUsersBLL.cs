using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
	public interface IUsersBLL
	{
		/// <summary>
		/// Adds user to a database
		/// </summary>
		/// <remarks>
		/// This method doesn't count SmallID field since it 
		/// managed by an SQL Database.
		/// </remarks>
		/// <param name="user">User DTO</param>
		/// <param name="credentials">Credentials DTO</param>
		/// <returns>Whether operation of creation was successful</returns>
		bool AddUser(UserDTO user, UserCredentialsDTO credentials);
		/// <summary>
		/// Updates user info in a database
		/// </summary>
		/// <remarks>
		/// This method doesn't count SmallID field since it 
		/// managed by an SQL Database.
		/// </remarks>
		/// <param name="user">User DTO with the same ID and new data</param>
		bool UpdateUser(UserDTO user);
		/// <summary>
		/// Updates credentials info in database
		/// </summary>
		/// <param name="updatedCredentials">Credentials DTO with the same ID and new data</param>
		/// <returns>Whether the operation was successful</returns>
		bool UpdateUserCredentials(UserCredentialsDTO updatedCredentials);
		/// <summary>
		/// Removes user from a database.
		/// </summary>
		/// <param name="userId">GUID of user to delete</param>
		bool RemoveUser(Guid userId);

		/// <summary>
		/// Gets all users from database
		/// </summary>
		/// <returns>Users' DTOs</returns>
		IEnumerable<UserDTO> GetUsers();
		/// <summary>
		/// Gets all users from database with a certain role
		/// </summary>
		/// <param name="roleId">GUID of a role</param>
		/// <returns>Users' DTOs</returns>
		IEnumerable<UserDTO> GetUsers(Guid roleId);
		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userId">GUID of user to get</param>
		/// <returns>DTO of a user</returns>
		UserDTO GetUser(Guid userId);
		/// <summary>
		/// Gets a certain user from a database
		/// </summary>
		/// <param name="userShortId">Incremental ID (number) of user to get</param>
		/// <returns>DTO of a user, null if there's no such a user</returns>
		UserDTO GetUser(int userShortId);
		/// <summary>
		/// Checks if a user with these credentials exist in a database
		/// and returns it.
		/// </summary>
		/// <param name="credentials">Credentials to check for in a database</param>
		/// <returns>DTO of a user, null if there's no such a user</returns>
		UserDTO GetUser(UserCredentialsDTO credentials);
		/// <summary>
		/// Gets login and password hash
		/// </summary>
		/// <param name="credentialsId">user's credentials id</param>
		/// <returns>Credentials DTO</returns>
		UserCredentialsDTO GetCredentials(Guid credentialsId);
		
		/// <summary>
		/// Searches for the user in database by it's nickname
		/// </summary>
		/// <param name="searchInput">Search string</param>
		/// <returns>Collection of users whose nickname matches the criteria</returns>
		IEnumerable<UserDTO> SearchUsers(string searchInput);
		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		bool IsLoginExist(string login);
	}
}

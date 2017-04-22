using System;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
	public interface IUserCredentialsDAL
	{
		/// <summary>
		/// Gets login and password hash
		/// </summary>
		/// <param name="id">credentials id</param>
		/// <returns>Credentials DTO</returns>
		UserCredentialsOutDTO GetCredentials(Guid id);
		/// <summary>
		/// Searches credentials info in db, and if found, returns user
		/// </summary>
		/// <param name="credentials">Credentials to search for</param>
		/// <returns>null if credentials aren't valid, UserDTO otherwise</returns>
		UserDTO CheckCredentials(UserCredentialsOutDTO credentials);
		/// <summary>
		/// Creates credentials in db
		/// </summary>
		/// <param name="credentials">Sign in info</param>
		/// <returns>Whether creation is successful</returns>
		bool AddCredentials(UserCredentialsOutDTO credentials);
		/// <summary>
		/// Removes user credentials from DB. 
		/// </summary>
		/// <remarks>
		/// Be careful, it also will delete user that belongs to them as long 
		/// as it's useless if you can't log in
		/// </remarks>
		/// <param name="id">credentials id</param>
		/// <returns>Whether credentials was deleted</returns>
		bool RemoveCredentials(Guid id);
		/// <summary>
		/// Updates sign in info
		/// </summary>
		/// <param name="credentials">credentials id</param>
		/// <returns>Whether there is credentials with this id, and it was updated</returns>
		bool UpdateCredentials(UserCredentialsOutDTO credentials);
		/// <summary>
		/// Checks for login in db. Returns whether login is exist or not
		/// </summary>
		/// <param name="login">login string</param>
		/// <returns>Whether login is exist or not</returns>
		bool IsLoginExist(string login);
	}
}
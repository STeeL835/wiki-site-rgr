using System;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
    public interface IUserCredentialsBLL
    {
		/// <summary>
		/// Creates credentials in db
		/// </summary>
		/// <param name="credentials">Sign in info</param>
		/// <returns>Whether creation is successful</returns>
		bool AddCredentials(UserCredentialsDTO credentials);
		/// <summary>
		/// Updates sign in info
		/// </summary>
		/// <param name="credentials">credentials id</param>
		/// <returns>Whether there is credentials with this id, and it was updated</returns>
		bool UpdateCredentials(UserCredentialsDTO credentials);
		/// <summary>
		/// Searches credentials info in db, and if found, returns user
		/// </summary>
		/// <param name="credentials">Credentials to search for</param>
		/// <returns>null if credentials aren't valid, UserDTO otherwise</returns>
		UserDTO CheckCredentials(UserCredentialsDTO credentials);
	}
}

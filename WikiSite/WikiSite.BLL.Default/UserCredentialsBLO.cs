using System;
using System.Collections.Generic;
using System.Diagnostics;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
	public class UserCredentialsBLO : IUserCredentialsBLL
	{
		private IUserCredentialsDAL _dal;

		public UserCredentialsBLO(IUserCredentialsDAL dal)
		{
			if (dal == null) throw new ArgumentNullException(nameof(dal), "DAL instance is null");
			_dal = dal;
		}

		/// <summary>
		/// Creates credentials in db
		/// </summary>
		/// <param name="credentials">Sign in info</param>
		/// <returns>Whether creation is successful</returns>
		public bool AddCredentials(UserCredentialsDTO credentials)
		{
			CheckThrowDTO(credentials);
			return _dal.AddCredentials(credentials);
		}

		/// <summary>
		/// Updates sign in info
		/// </summary>
		/// <param name="credentials">credentials id</param>
		/// <returns>Whether there is credentials with this id, and it was updated</returns>
		public bool UpdateCredentials(UserCredentialsDTO credentials)
		{
			CheckThrowDTO(credentials);
			return _dal.UpdateCredentials(credentials);
		}

		/// <summary>
		/// Searches credentials info in db, and if found, returns user
		/// </summary>
		/// <param name="credentials">Credentials to search for</param>
		/// <returns>null if credentials aren't valid, UserDTO otherwise</returns>
		public UserDTO CheckCredentials(UserCredentialsDTO credentials)
		{
			CheckThrowDTO(credentials);
			return _dal.CheckCredentials(credentials);
		}

		private void CheckThrowDTO(UserCredentialsDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto), "UserCredentialsDTO mustn't be null");

			if (string.IsNullOrWhiteSpace(dto.Login)) throw new ArgumentException("Login can't be null or empty");
			if (dto.PasswordHash == null || dto.PasswordHash.Length == 0) throw new ArgumentException("Password hash mustn't be null or empty");
		}
	}
}
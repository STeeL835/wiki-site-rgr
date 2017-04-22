using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
	public class RolesDAO : IRolesDAL
	{
		private static readonly string ConnectionString;

		static RolesDAO()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
		}

		/// <summary>
		/// Returns a role with certain id
		/// </summary>
		/// <param name="id">Guid of a role</param>
		/// <returns>DTO of a role</returns>
		public RoleDTO GetRole(Guid id)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Roles] " +
				                            "WHERE Id = @id", connection);
				sqlCom.Parameters.AddWithValue("@id", id);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					return new RoleDTO
					{
						Id = (Guid)reader["Id"],
						Name = (string)reader["Name"]
					};
				}
			}
			throw new EntryNotFoundException($"Role with id {id} was not found");
		}

		/// <summary>
		/// Returns all the roles that exist in db
		/// </summary>
		/// <returns>Roles sequence</returns>
		public IEnumerable<RoleDTO> GetRoles()
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				var sqlCom = new SqlCommand("SELECT * FROM [Roles]", connection);

				connection.Open();
				var reader = sqlCom.ExecuteReader();
				while (reader.Read())
				{
					yield return new RoleDTO
					{
						Id = (Guid)reader["Id"],
						Name = (string)reader["Name"]
					};
				}
			}
		}
	}
}
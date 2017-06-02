using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
    public class ImagesDAO: IImagesDAL
    {
        private static readonly string ConnectionString;

        static ImagesDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
        }
        /// <summary>
        /// Adds image to database.
        /// </summary>
        /// <param name="image">Image DTO</param>
        /// <returns></returns>
        public bool AddImage(ImageDTO image)
        {
            int addedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("INSERT INTO [Images] (Id, Data, Type) VALUES(@id, @data, @type)", connection);
                sqlCom.Parameters.AddWithValue("@id", image.Id);
                sqlCom.Parameters.AddWithValue("@data", image.Data);
                sqlCom.Parameters.AddWithValue("@type", image.Type);
                connection.Open();

                addedRows = sqlCom.ExecuteNonQuery();
            }

            return addedRows == 1;
        }

        /// <summary>
        /// Gets image from database.
        /// </summary>
        /// <param name="id">Guid of image</param>
        /// <returns></returns>
        public ImageDTO GetImage(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Images] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", id);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ImageDTO
                    {
                        Id = (Guid)reader["Id"],
                        Data = (byte[])reader["Data"],
                        Type = (string)reader["Type"]
                    };
                }
            }
            throw new EntryNotFoundException($"Image with id {id} has not found.");
        }

        /// <summary>
        /// Removes image from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveImage(Guid id)
        {
            int affectedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("DELETE FROM [Images] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", id);
                connection.Open();

                affectedRows = sqlCom.ExecuteNonQuery();
            }

            return affectedRows == 1;
        }

        /// <summary>
        /// Gets all images from database;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageDTO> GetAllImage()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Images]", connection);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ImageDTO
                    {
                        Id = (Guid)reader["Id"],
                        Data = (byte[])reader["Data"],
                        Type = (string)reader["Type"]
                    };
                }
            }
        }
    }
}

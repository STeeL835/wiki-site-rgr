using System;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
    public class ArticleContentsDAO : IArticleContentsDAL
    {
        private static readonly string ConnectionString;

        static ArticleContentsDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
        }

        /// <summary>
        /// Adds article's content to database.
        /// </summary>
        /// <param name="content">Article's content DTO</param>
        public bool AddContent(ArticleContentDTO content)
        {
            if (content == null) throw new ArgumentNullException();

            int addedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("INSERT INTO [ArticleContents] (Id, Definition, Text) VALUES(@id, @definition, @text)", connection);
                sqlCom.Parameters.AddWithValue("@id", content.Id);
                sqlCom.Parameters.AddWithValue("@definition", content.Definition);
                sqlCom.Parameters.AddWithValue("@text", content.Text);
                connection.Open();

                addedRows = sqlCom.ExecuteNonQuery();
            }

            return addedRows == 1;
        }

        /// <summary>
        /// Gets article's content from database.
        /// </summary>
        /// <param name="contentId">GUID of article's content to get</param>
        /// <returns>DTO of a article's content</returns>
        public ArticleContentDTO GetContent(Guid contentId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [ArticleContents] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", contentId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleContentDTO
                    {
                        Id = (Guid) reader["Id"],
                        Definition = (string) reader["Definition"],
                        Text = (string) reader["Text"]
                    };
                }
            }
            throw new EntryNotFoundException($"Article content with id {contentId} was not found");
        }

        /// <summary>
        /// Removes article's content from database.
        /// </summary>
        /// <param name="contentId">GUID of article's content to delete</param>
        public bool RemoveContent(Guid contentId)
        {
            int affectedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("DELETE FROM [ArticleContents] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", contentId);
                connection.Open();

                affectedRows = sqlCom.ExecuteNonQuery();
            }

            return affectedRows == 1;
        }
    }
}
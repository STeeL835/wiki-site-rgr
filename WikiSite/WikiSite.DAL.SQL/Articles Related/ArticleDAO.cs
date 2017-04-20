using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;


namespace WikiSite.DAL.SQL
{
    public class ArticleDAO : IArticlesDAL
    {
        private static readonly string _connectionString;

        static ArticleDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
        }

        /// <summary>
        /// Adds article to a database.
        /// </summary>
        /// <remarks>
        /// This method doesn't count SmallID field since it managed by an SQL Database.
        /// </remarks>
        /// <param name="article">Article DTO</param>
        public bool AddArticle(ArticleDTO article)
        {
            if (article == null) throw new ArgumentNullException();

            int addedRows;
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlCom = new SqlCommand("INSERT INTO [Article] (Id, Short_Url, Author_Id, Heading, Date_Of_Creation)" + 
                                            "VALUES(@id, @short_url @author_id, @heading, @date_of_creation)", connection);
                sqlCom.Parameters.AddWithValue("@id", article.Id);
                sqlCom.Parameters.AddWithValue("@short_urll", article.ShortUrl);
                sqlCom.Parameters.AddWithValue("@author_id", article.AuthorId);
                sqlCom.Parameters.AddWithValue("@heading", article.Heading);
                sqlCom.Parameters.AddWithValue("@date_of_creation", article.CreationDate);
                connection.Open();

                addedRows = sqlCom.ExecuteNonQuery();
            }

            return addedRows == 1;
        }

        /// <summary>
        /// Gets all articles from database.
        /// </summary>
        /// <returns>Articles' DTOs</returns>
        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a certain article from a database.
        /// </summary>
        /// <param name="articleShortId">Incremental ID (number) of article to get</param>
        /// <returns>DTO of a article</returns>
        public ArticleDTO GetArticle(string shortUrl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a certain article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of a article</returns>
        public ArticleDTO GetArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Removes article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        public bool RemoveArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDTO> SearchArticles(string searchInput)
        {
            throw new NotImplementedException();
        }
    }
}

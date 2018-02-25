using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;


namespace WikiSite.DAL.SQL
{
    public class ArticlesDAO : IArticlesDAL
    {
        private static readonly string ConnectionString;

        static ArticlesDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
        }

        /// <summary>
        /// Adds article to database.
        /// </summary>
        /// <remarks>
        /// This method doesn't count SmallID field since it managed by an SQL Database.
        /// </remarks>
        /// <param name="article">Article DTO</param>
        public bool AddArticle(ArticleDTO article)
        {
            if (article == null) throw new ArgumentNullException();

            int addedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("INSERT INTO [Articles] (Id, Short_Url, Author_Id, Date_Of_Creation) VALUES(@id, @short_url, @author_id, @date_of_creation)", connection);
                sqlCom.Parameters.AddWithValue("@id", article.Id);
                sqlCom.Parameters.AddWithValue("@short_url", article.ShortUrl);
                sqlCom.Parameters.AddWithValue("@author_id", article.AuthorId);
                sqlCom.Parameters.AddWithValue("@date_of_creation", article.CreationDate);
                connection.Open();

                addedRows = sqlCom.ExecuteNonQuery();
            }

            return addedRows == 1;
        }

        /// <summary>
        /// Returns all articles from database.
        /// </summary>
        /// <returns>Articles' DTOs</returns>
        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Articles]", connection);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ArticleDTO
                    {
                        Id = (Guid) reader["Id"],
                        ShortUrl = (string) reader["Short_Url"],
                        AuthorId = (Guid) reader["Author_Id"],
                        CreationDate = (DateTime) reader["Date_Of_Creation"]
                    };
                }
            }
        }

        /// <summary>
        /// Returns all articles form database, which created by author.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' DTOs</returns>
        public IEnumerable<ArticleDTO> GetAllArticles(Guid authorId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Articles] WHERE Author_Id = @author_id", connection);
                sqlCom.Parameters.AddWithValue("@author_id", authorId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ArticleDTO
                    {
                        Id = (Guid)reader["Id"],
                        ShortUrl = (string)reader["Short_Url"],
                        AuthorId = (Guid)reader["Author_Id"],
                        CreationDate = (DateTime)reader["Date_Of_Creation"]
                    };
                }
            }
        }

        /// <summary>
        /// Returns a certain article from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>Article DTO</returns>
        public ArticleDTO GetArticle(string shortUrl)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Articles] WHERE Short_Url = @short_url", connection);
                sqlCom.Parameters.AddWithValue("@short_url", shortUrl);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleDTO
                    {
                        Id = (Guid) reader["Id"],
                        ShortUrl = (string) reader["Short_Url"],
                        AuthorId = (Guid) reader["Author_Id"],
                        CreationDate = (DateTime) reader["Date_Of_Creation"]
                    };
                }
            }
            throw new EntryNotFoundException($"Article with url {shortUrl} has not found.");
        }

        /// <summary>
        /// Returns a certain article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Article DTO</returns>
        public ArticleDTO GetArticle(Guid articleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [Articles] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", articleId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleDTO
                    {
                        Id = (Guid) reader["Id"],
                        ShortUrl = (string) reader["Short_Url"],
                        AuthorId = (Guid) reader["Author_Id"],
                        CreationDate = (DateTime) reader["Date_Of_Creation"]
                    };
                }
            }
            throw new EntryNotFoundException($"Article with id {articleId} has not found.");
        }

        /// <summary>
        /// Returns a guide article from database.
        /// </summary>
        /// <returns></returns>
        public ArticleDTO GetGuideArticle()
        {
            return GetArticle(ConfigurationManager.AppSettings["GuideShortUrl"]);
        }

        /// <summary>
        /// Returns a random article from database.
        /// </summary>
        /// <returns></returns>
        public ArticleDTO GetRandomArticle() //TODO: FIX
        {
            var i = 0;
            while (i < 3)
            {
                var tempdata = new ArticleDTO();
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var sqlCom = new SqlCommand("SELECT TOP 1 * FROM [Articles] ORDER BY NEWID()", connection);
                    connection.Open();
                    var reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                        tempdata = new ArticleDTO
                        {
                            Id = (Guid)reader["Id"],
                            ShortUrl = (string)reader["Short_Url"],
                            AuthorId = (Guid)reader["Author_Id"],
                            CreationDate = (DateTime)reader["Date_Of_Creation"]
                        };
                        if (tempdata.ShortUrl != ConfigurationManager.AppSettings["GuideShortUrl"])
                            return tempdata;
                        else i++;
                    }
                }
                if (i >= 3)
                {
                    var articles = GetAllArticles().ToList();
                    if (articles.Count > 1)
                        i = 0;
                    else if (articles.Count == 1)
                        return tempdata;
                }
            }
            throw new EntryNotFoundException("Article has not found.");
        }

        /// <summary>
        /// Removes article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        public bool RemoveArticle(Guid articleId)
        {
            int affectedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("DELETE FROM [Articles] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", articleId);
                connection.Open();

                affectedRows = sqlCom.ExecuteNonQuery();
            }

            return affectedRows == 1;
        }
    }
}
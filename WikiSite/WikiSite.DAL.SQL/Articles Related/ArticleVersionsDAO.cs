using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.DAL.SQL
{
    public class ArticleVersionsDAO : IArticleVersionsDAL
    {
        private static readonly string ConnectionString;

        static ArticleVersionsDAO()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["WikiSiteDB"].ConnectionString;
        }

        /// <summary>
        /// Adds article's version to database.
        /// </summary>
        /// <param name="version">Version DTO</param>
        public bool AddVersion(ArticleVersionDTO version)
        {
            if (version == null) throw new ArgumentNullException();

            int addedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("INSERT INTO [ArticleVersions] (Id, Article_Id, Content_Id, Date_Of_Edition, Editor_Id, Is_Approved) VALUES(@id, @article_id, @content_id, @date_of_edition, @editor_id, @is_approved)", connection);
                sqlCom.Parameters.AddWithValue("@id", version.Id);
                sqlCom.Parameters.AddWithValue("@article_id", version.ArticleId);
                sqlCom.Parameters.AddWithValue("@content_id", version.ContentId);
                sqlCom.Parameters.AddWithValue("@date_of_edition", version.LastEditDate);
                sqlCom.Parameters.AddWithValue("@editor_id", version.EditionAuthorId);
                sqlCom.Parameters.AddWithValue("@is_approved", version.IsApproved);
                connection.Open();

                addedRows = sqlCom.ExecuteNonQuery();
            }

            return addedRows == 1;
        }

        /// <summary>
        /// Removes article's version to database.
        /// </summary>
        /// <param name="versionId">GUID of version to delete</param>
        public bool RemoveVersion(Guid versionId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM [ArticleVersions] WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", versionId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        /// <summary>
        /// Gets all article's versions from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Article's versions DTOs</returns>
        public IEnumerable<ArticleVersionDTO> GetAllVersions(Guid articleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [ArticleVersions] WHERE Article_Id = @article_id", connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);
                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
        }

        /// <summary>
        /// Gets all version, which created by author, from database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Article's versions DTOs</returns>
        public IEnumerable<ArticleVersionDTO> GetAllVersionsByAuthor(Guid authorId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [ArticleVersions] WHERE Editor_Id = @editor_id", connection);
                sqlCom.Parameters.AddWithValue("@editor_id", authorId);
                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ArticleVersionDTO
                    {
                        Id = (Guid)reader["Id"],
                        ArticleId = (Guid)reader["Article_Id"],
                        ContentId = (Guid)reader["Content_Id"],
                        LastEditDate = (DateTime)reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid)reader["Editor_Id"],
                        IsApproved = (bool)reader["Is_Approved"]
                    };
                }
            }
        }

        /// <summary>
        /// Gets a certain article's version by id from database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        /// <returns>DTO of finded article's version</returns>
        public ArticleVersionDTO GetVersion(Guid versionId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [ArticleVersions] WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", versionId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
            throw new EntryNotFoundException(
                $"Version with id {versionId} has not found.");
        }

        /// <summary>
        /// Gets a certain article's version by date from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        public ArticleVersionDTO GetVersion(Guid articleId, DateTime date)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM [ArticleVersions] WHERE (Article_Id = @article_id AND Date_Of_Edition = @date_of_edition)", connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);
                sqlCom.Parameters.AddWithValue("@date_of_edition", date);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
            throw new EntryNotFoundException($"Version for article with id {articleId} by time {date} has not found.");
        }

        /// <summary>
        /// Gets article's version by number from sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="number">Number of verson of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        public ArticleVersionDTO GetVersion(Guid articleId, int number)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Date_Of_Edition) AS Number, * FROM [ArticleVersions] WHERE(Article_Id = @article_id)) X WHERE Number = @number", connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);
                sqlCom.Parameters.AddWithValue("@number", number);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
            throw new EntryNotFoundException($"Version for article with id {articleId} by number {number} has not found.");
        }

        /// <summary>
        /// Gets last approved article's version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of last approved article's version</returns>
        public ArticleVersionDTO GetLastApprovedVersion(Guid articleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT top 1 * FROM [ArticleVersions] WHERE (Article_Id = @article_id AND Is_Approved = @is_approved) ORDER BY Date_Of_Edition DESC", connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);
                sqlCom.Parameters.AddWithValue("@is_approved", true);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
            throw new EntryNotFoundException(
                $"The Latest approved version for article with id {articleId} has not found.");
        }

        /// <summary>
        /// Gets last article's version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of last article's version</returns>
        public ArticleVersionDTO GetLastVersion(Guid articleId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT top 1 * FROM [ArticleVersions] WHERE (Article_Id = @article_id) ORDER BY Date_Of_Edition DESC", connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return new ArticleVersionDTO
                    {
                        Id = (Guid) reader["Id"],
                        ArticleId = (Guid) reader["Article_Id"],
                        ContentId = (Guid) reader["Content_Id"],
                        LastEditDate = (DateTime) reader["Date_Of_Edition"],
                        EditionAuthorId = (Guid) reader["Editor_Id"],
                        IsApproved = (bool) reader["Is_Approved"]
                    };
                }
            }
            throw new EntryNotFoundException($"The Latest version for article with id {articleId} has not found.");
        }

        /// <summary>
        /// Approves a certain article's version by id in database.
        /// </summary>
        /// <param name="versionId">GUID of version to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersion(Guid versionId, bool value = true)
        {
            int affectedRows;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("UPDATE [ArticleVersions] SET Is_Approved = @is_approved WHERE Id = @id", connection);
                sqlCom.Parameters.AddWithValue("@id", versionId);
                sqlCom.Parameters.AddWithValue("@is_approved", value);
                connection.Open();

                affectedRows = sqlCom.ExecuteNonQuery();
            }

            return affectedRows == 1;
        }

        /// <summary>
        /// Approves a certain article's version by date in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersion(Guid articleId, DateTime date, bool value = true)
        {
            var version = GetVersion(articleId, date);
            return ApproveVersion(version.Id, value);
        }

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersion(Guid articleId, int number, bool value = true)
        {
            var version = GetVersion(articleId, number);
            return ApproveVersion(version.Id, value);
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        public int GetNumberOfVersion(Guid versionId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Date_Of_Edition) AS Number, * FROM [ArticleVersions] WHERE(Article_Id = @article_id)) X Id = @id",
                    connection);
                sqlCom.Parameters.AddWithValue("@article_id", GetVersion(versionId).ArticleId);
                sqlCom.Parameters.AddWithValue("@id", versionId);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return (int)reader["Number"];
                }
            }
            throw new EntryNotFoundException();
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        public int GetNumberOfVersion(Guid articleId, DateTime date)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlCom = new SqlCommand("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Date_Of_Edition) AS Number, * FROM [ArticleVersions] WHERE(Article_Id = @article_id)) X Id = @id",
                    connection);
                sqlCom.Parameters.AddWithValue("@article_id", articleId);
                sqlCom.Parameters.AddWithValue("@id", GetVersion(articleId, date).Id);

                connection.Open();
                var reader = sqlCom.ExecuteReader();
                while (reader.Read())
                {
                    return (int)reader["Number"];
                }
            }
            throw new EntryNotFoundException($"Version for article with id {articleId} by time {date} has not found.");
        }
    }
}
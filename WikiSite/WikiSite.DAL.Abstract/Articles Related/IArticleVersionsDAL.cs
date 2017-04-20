using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IArticleVersionsDAL
    {
        /// <summary>
        /// Adds article's version to a database.
        /// </summary>
        /// <param name="article">Version DTO</param>
        bool AddVersion(ArticleVersionDTO version);
        /// <summary>
        /// Removes article's version to a database.
        /// </summary>
        /// <param name="articleId">GUID of version to delete</param>
        bool RemoveVersion(Guid versionId);
        /// <summary>
		/// Gets all article's versions from database.
		/// </summary>
        /// <param name="articleId">GUID of article to get</param>
		/// <returns>Article's versions DTOs</returns>
	    IEnumerable<ArticleVersionDTO> GetAllVersions(Guid articleId);
        /// <summary>
        /// Gets article's version by date_time from database.
        /// </summary>
        /// <param name="date">DateTime of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        ArticleVersionDTO GetVersionByDateTime(Guid articleId, DateTime date);
        /// <summary>
        /// Gets article's version by number from sorted database by date_time.
        /// </summary>
        /// <param name="number">Number of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        ArticleVersionDTO GetVersionByNumber(Guid articleId, int number);
        /// <summary>
        /// Gets last article's version from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of last article's version</returns>
        ArticleVersionDTO GetLastVersion(Guid articleId);
        /// <summary>
        /// Gets last approved article's version from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of last approved article's version</returns>
        ArticleVersionDTO GetLastApprovedVersion(Guid articleId);
    }
}

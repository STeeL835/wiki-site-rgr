using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
    public interface IArticlesBLL
    {
        /// <summary>
        /// Adds new article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        /// <returns></returns>
        bool AddArticle(ArticleBDO article);

        /// <summary>
        /// Removes article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        /// <returns></returns>
        bool RemoveArticle(Guid articleId);

        /// <summary>
        /// Adds new NOT approved version of article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        bool UpdateArticle(ArticleBDO article);

        /// <summary>
        /// Returns article in last edit version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        ArticleBDO GetArticle(Guid articleId);

        /// <summary>
        /// Returns article in last edit version from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        ArticleBDO GetArticle(string shortUrl);

        /// <summary>
        /// Returns a certain last edit version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of last edit version of a article</returns>
        ArticleBDO GetLastVersionOftArticle(Guid articleId);

        /// <summary>
        /// Returns a certain last approved version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of last approved version of a article</returns>
        ArticleBDO GetLastApprovedVersionOfArticle(Guid articleId);

        /// <summary>
        /// Get a certain version of article from database.
        /// </summary>
        /// <param name="articleVersionId">GUID of version of article to get</param>
        /// <returns>Article BDO</returns>
        ArticleBDO GetVersionOfArticle(Guid articleVersionId);

        /// <summary>
        /// Returns a certain version of article from database.
        /// </summary>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>Article BDO</returns>
        ArticleBDO GetVersionOfArticle(Guid articleId, DateTime date);

        /// <summary>
        /// Returns a certain version of article from database.
        /// </summary>
        /// <remarks>
        /// Number is calculated by date.
        /// </remarks>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>Article BDO</returns>
        ArticleBDO GetVersionOfArticle(Guid articleId, int number);

        /// <summary>
        /// Returns all articles form database.
        /// </summary>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllArticles();

        /// <summary>
        /// Returns all articles, which created by author, form database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllArticles(Guid authorId);

        /// <summary>
        /// Returns all version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllVersionOfArticle(Guid articleId);

        /// <summary>
        /// Returns all version, which created by author, from database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllVersionByAuthor(Guid authorId);

        /// <summary>
        /// Approves a certain version of article from database.
        /// </summary>
        /// <param name="versionId">>GUID of version to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid versionId, bool value = true);

        /// <summary>
        /// Approves a certain article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid articleId, DateTime date, bool value = true);

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid articleId, int number, bool value = true);

        /// <summary>
        /// Returns a number of versions for article.
        /// </summary>
        /// <param name="articleId">Artcle to count</param>
        /// <returns>Number of versions</returns>
        int VersionsCount(Guid articleId);

        /// <summary>
        /// Checks for short url in database.
        /// </summary>
        /// <param name="shortUrl">Checked short url</param>
        /// <returns>Whether article with short url is exist or not</returns>
        bool IsShortUrlExist(string shortUrl);

        /// <summary>
        /// Returns a random article from database.
        /// </summary>
        /// <returns></returns>
        ArticleBDO GetRandomArticle();

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        int GetNumberOfVersion(Guid versionId);

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        int GetNumberOfVersion(Guid articleId, DateTime date);

		/// <summary>
		/// Performs search in all latest versions' headers, then texts 
		/// and returns versions that apply the query
		/// </summary>
		/// <param name="query">text to search in article text</param>
		/// <returns>ArticleVersions DTOs</returns>
		IEnumerable<ArticleBDO> SearchArticles(string query);

        /// <summary>
        /// Returns a guide article from database.
        /// </summary>
        /// <returns></returns>
        ArticleBDO GetGuideArticle();
    }
}
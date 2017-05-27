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
        /// Gets article in last edit version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        ArticleBDO GetArticle(Guid articleId);

        /// <summary>
        /// Gets article in last edit version from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        ArticleBDO GetArticle(string shortUrl);

        /// <summary>
        /// Gets a certain last edit version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of last edit version of a article</returns>
        ArticleBDO GetLastVersionOftArticle(Guid articleId);

        /// <summary>
        /// Gets a certain last approved version of article from database.
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
        /// Gets a certain version of article from database.
        /// </summary>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>Article BDO</returns>
        ArticleBDO GetVersionOfArticle(Guid articleId, DateTime date);

        /// <summary>
        /// Gets a certain version of article from database.
        /// </summary>
        /// <remarks>
        /// Number is calculated by date.
        /// </remarks>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>Article BDO</returns>
        ArticleBDO GetVersionOfArticle(Guid articleId, int number);

        /// <summary>
        /// Gets all articles form database.
        /// </summary>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllArticles();

        /// <summary>
        /// Gets all articles, which created by author, form database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllArticles(Guid authorId);

        /// <summary>
        /// Gets all version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllVersionOfArticle(Guid articleId);

        /// <summary>
        /// Gets all version, which created by author, from database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        IEnumerable<ArticleBDO> GetAllVersionByAuthor(Guid authorId);

        /// <summary>
        /// Approves a certain version of article from database.
        /// </summary>
        /// <param name="versionId">>GUID of version to approve</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid versionId);

        /// <summary>
        /// Approves a certain article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid articleId, DateTime date);

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <returns></returns>
        bool ApproveVersionOfArticle(Guid articleId, int number);

        /// <summary>
        /// Returns a number of versions for article.
        /// </summary>
        /// <param name="articleId">Artcle to count</param>
        /// <returns>Number of versions</returns>
        int VersionsCount(Guid articleId);

        /// <summary>
        /// Checks for login in database.
        /// </summary>
        /// <param name="shortUrl">Checked short url</param>
        /// <returns>Whether heading is exist or not</returns>
        bool IsShortUrlExist(string shortUrl);
    }
}
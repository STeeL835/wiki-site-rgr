using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IArticlesDAL
    {
        /// <summary>
        /// Adds article to database.
        /// </summary>
        /// <param name="article">Article DTO</param>
        bool AddArticle(ArticleDTO article);

        /// <summary>
        /// Removes article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        bool RemoveArticle(Guid articleId);

        /// <summary>
        /// Gets a certain article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Article DTO</returns>
        ArticleDTO GetArticle(Guid articleId);

        /// <summary>
        /// Gets a certain article from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>Article DTO</returns>
        ArticleDTO GetArticle(string shortUrl);

        /// <summary>
        /// Gets all articles from database.
        /// </summary>
        /// <returns>Articles' DTOs</returns>
        IEnumerable<ArticleDTO> GetAllArticles();

        /// <summary>
        /// Gets all articles form database, which created by author.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' DTOs</returns>
        IEnumerable<ArticleDTO> GetAllArticles(Guid authorId);
    }
}
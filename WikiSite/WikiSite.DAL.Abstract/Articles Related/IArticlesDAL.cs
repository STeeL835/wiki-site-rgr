using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IArticlesDAL
    {
        /// <summary>
        /// Adds article to a database.
        /// </summary>
        /// <param name="article">Article DTO</param>
        bool AddArticle(ArticleDTO article);

        /// <summary>
        /// Removes article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        bool RemoveArticle(Guid articleId);

        /// <summary>
        /// Gets a certain article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>DTO of a article</returns>
        ArticleDTO GetArticle(Guid articleId);

        /// <summary>
        /// Gets a certain article from a database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>DTO of a article</returns>
        ArticleDTO GetArticle(string shortUrl);

        /// <summary>
        /// Gets all articles from database.
        /// </summary>
        /// <returns>Articles' DTOs</returns>
        IEnumerable<ArticleDTO> GetAllArticles();
    }
}
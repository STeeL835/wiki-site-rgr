﻿using System;
using System.Collections.Generic;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IArticleVersionsDAL
    {
        /// <summary>
        /// Adds article's version to a database.
        /// </summary>
        /// <param name="version">Version DTO</param>
        bool AddVersion(ArticleVersionDTO version);

        /// <summary>
        /// Removes article's version to a database.
        /// </summary>
        /// <param name="versionId">GUID of version to delete</param>
        bool RemoveVersion(Guid versionId);

        /// <summary>
        /// Gets all article's versions from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Article's versions DTOs</returns>
        IEnumerable<ArticleVersionDTO> GetAllVersions(Guid articleId);

        /// <summary>
        /// Gets a certain article's version by id from database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        /// <returns>DTO of finded article's version</returns>
        ArticleVersionDTO GetVersion(Guid versionId);

        /// <summary>
        /// Gets a certain article's version by date time from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        ArticleVersionDTO GetVersion(Guid articleId, DateTime date);

        /// <summary>
        /// Gets a certain article's version by number from sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>DTO of finded article's version</returns>
        ArticleVersionDTO GetVersion(Guid articleId, int number);

        /// <summary>
        /// Gets a certain last article's version from a database.
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

        /// <summary>
        /// Approves a certain article's version by id in database.
        /// </summary>
        /// <param name="versionId">GUID of version to approve</param>
        /// <returns></returns>
        bool ApproveVersion(Guid versionId);

        /// <summary>
        /// Approves a certain article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <returns></returns>
        bool ApproveVersion(Guid articleId, DateTime date);

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <returns></returns>
        bool ApproveVersion(Guid articleId, int number);
    }
}
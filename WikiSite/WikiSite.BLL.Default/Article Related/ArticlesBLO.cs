using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;
using WikiSite.Caretakers;

namespace WikiSite.BLL.Default
{
    public class ArticleBLO : IArticlesBLL
    {
        private IArticlesDAL _articlesDAL;
        private IArticleVersionsDAL _articleVersionsDAL;
        private IArticleContentsDAL _articleContentsDAL;

        public ArticleBLO(IArticlesDAL articlesDAL, IArticleVersionsDAL articleVersionsDAL,
            IArticleContentsDAL articleContentsDAL)
        {
            if (articlesDAL == null)
                throw new ArgumentNullException(nameof(articlesDAL), "Articles DAL instance is null.");
            if (articleVersionsDAL == null)
                throw new ArgumentNullException(nameof(articleVersionsDAL), "Article Versions DAL instance is null.");
            if (articleContentsDAL == null)
                throw new ArgumentNullException(nameof(articleContentsDAL), "Article Contents DAL instance is null.");
            _articlesDAL = articlesDAL;
            _articleVersionsDAL = articleVersionsDAL;
            _articleContentsDAL = articleContentsDAL;
        }

        /// <summary>
        /// Adds new article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        /// <returns></returns>
        public bool AddArticle(ArticleBDO article)
        {
            ErrorGuard.Check(article);
            var articleDTO = CreateArticleDTO(article);
            var contentDTO = CreateArticleContentDTO(article);
            var versionDTO = CreateArticleVersionDTO(article, contentDTO);
            return _articlesDAL.AddArticle(articleDTO) && _articleContentsDAL.AddContent(contentDTO) &&
                   _articleVersionsDAL.AddVersion(versionDTO);
        }

        /// <summary>
        /// Removes article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        /// <returns></returns>
        public bool RemoveArticle(Guid articleId)
        {
            ErrorGuard.Check(articleId);
            var versionDTOs = _articleVersionsDAL.GetAllVersions(articleId);
            var temp = true;
            foreach (var version in versionDTOs)
            {
                temp = temp && _articleVersionsDAL.RemoveVersion(version.Id);
                temp = temp && _articleContentsDAL.RemoveContent(version.ContentId);
            }
            return temp && _articlesDAL.RemoveArticle(articleId);
        }

        /// <summary>
        /// Adds new NOT approved version of article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        public bool UpdateArticle(ArticleBDO article)
        {
            ErrorGuard.Check(article);
            var contentDTO = CreateArticleContentDTO(article);
            var versionDTO = CreateArticleVersionDTO(article, contentDTO);
            return _articleContentsDAL.AddContent(contentDTO) && _articleVersionsDAL.AddVersion(versionDTO);
        }

        /// <summary>
        /// Gets article in last edit version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        public ArticleBDO GetArticle(Guid articleId)
        {
            ErrorGuard.Check(articleId);
            var article = _articlesDAL.GetArticle(articleId);
            return CreateArticleBDO(_articleVersionsDAL.GetLastVersion(article.Id));
        }

        /// <summary>
        /// Gets article in last edit version from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        public ArticleBDO GetArticle(string shortUrl)
        {
            ErrorGuard.Check(shortUrl);
            var article = _articlesDAL.GetArticle(shortUrl);
            return CreateArticleBDO(_articleVersionsDAL.GetLastVersion(article.Id));
        }

        /// <summary>
        /// Gets a certain last edit version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BDO of last edit version of a article</returns>
        public ArticleBDO GetLastVersionOftArticle(Guid articleId)
        {
            ErrorGuard.Check(articleId);
            return CreateArticleBDO(_articleVersionsDAL.GetLastVersion(articleId));
        }

        /// <summary>
        /// Gets a certain last approved version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BDO of last approved version of a article</returns>
        public ArticleBDO GetLastApprovedVersionOfArticle(Guid articleId)
        {
            ErrorGuard.Check(articleId);
            return CreateArticleBDO(_articleVersionsDAL.GetLastApprovedVersion(articleId));
        }

        /// <summary>
        /// Gets a certain version of article from database.
        /// </summary>
        /// <param name="articleVersionId">GUID of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleVersionId)
        {
            ErrorGuard.Check(articleVersionId);
            return CreateArticleBDO(_articleVersionsDAL.GetVersion(articleVersionId));
        }

        /// <summary>
        /// Gets a certain version of article from database.
        /// </summary>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleId, DateTime date)
        {
            ErrorGuard.Check(articleId);
            ErrorGuard.Check(date);
            return CreateArticleBDO(_articleVersionsDAL.GetVersion(articleId, date));
        }

        /// <summary>
        /// Gets a certain version of article from database.
        /// </summary>
        /// <remarks>
        /// Number is calculated by date.
        /// </remarks>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleId, int number)
        {
            ErrorGuard.Check(articleId);
            ErrorGuard.Check(number);
            return CreateArticleBDO(_articleVersionsDAL.GetVersion(articleId, number));
        }

        /// <summary>
        /// Gets all articles form database.
        /// </summary>
        /// <returns>Articles' BDOs</returns>
        public IEnumerable<ArticleBDO> GetAllArticles()
        {
            var articleDTOs = _articlesDAL.GetAllArticles().ToList();
            var versionDTOs = new List<ArticleVersionDTO>();
            var articleBDOs = new List<ArticleBDO>();
            foreach (var article in articleDTOs)
            {
                versionDTOs.Add(_articleVersionsDAL.GetLastVersion(article.Id));
            }
            if (articleDTOs.Count != versionDTOs.Count)
                throw new ApplicationException("Something has gone wrong with getting articles from database.");
            foreach (var version in versionDTOs)
            {
                articleBDOs.Add(CreateArticleBDO(version));
            }
            foreach (var article in articleBDOs)
            {
                yield return article;
            }
        }

        /// <summary>
        /// Gets all articles form database, which created by author.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        public IEnumerable<ArticleBDO> GetAllArticles(Guid authorId)
        {
            ErrorGuard.Check(authorId);
            var articleDTOs = _articlesDAL.GetAllArticles(authorId).ToList();
            var versionDTOs = new List<ArticleVersionDTO>();
            var articleBDOs = new List<ArticleBDO>();
            foreach (var article in articleDTOs)
            {
                versionDTOs.Add(_articleVersionsDAL.GetLastVersion(article.Id));
            }
            if (articleDTOs.Count != versionDTOs.Count)
                throw new ApplicationException("Something has gone wrong with getting articles from database.");
            foreach (var version in versionDTOs)
            {
                articleBDOs.Add(CreateArticleBDO(version));
            }
            foreach (var article in articleBDOs)
            {
                yield return article;
            }
        }

        /// <summary>
        /// Gets all version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Articles' BDOs</returns>
        public IEnumerable<ArticleBDO> GetAllVersionOfArticle(Guid articleId)
        {
            ErrorGuard.Check(articleId);
            var versionDTOs = _articleVersionsDAL.GetAllVersions(articleId);
            var articleBDOs = new List<ArticleBDO>();
            foreach (var version in versionDTOs)
            {
                articleBDOs.Add(CreateArticleBDO(version));
            }
            foreach (var article in articleBDOs)
            {
                yield return article;
            }
        }

        /// <summary>
        /// Gets all version, which created by author, from database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' BDOs</returns>
        public IEnumerable<ArticleBDO> GetAllVersionByAuthor(Guid authorId)
        {
            ErrorGuard.Check(authorId);
            var versionDTOs = _articleVersionsDAL.GetAllVersionsByAuthor(authorId);
            var articleBDOs = new List<ArticleBDO>();
            foreach (var version in versionDTOs)
            {
                articleBDOs.Add(CreateArticleBDO(version));
            }
            foreach (var article in articleBDOs)
            {
                yield return article;
            }
        }

        /// <summary>
        /// Approves a certain version of article from database.
        /// </summary>
        /// <param name="versionId">>GUID of version to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid versionId, bool value = true)
        {
            return _articleVersionsDAL.ApproveVersion(versionId, value);
        }

        /// <summary>
        /// Approves a certain article's version by date in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid articleId, DateTime date, bool value = true)
        {
            return _articleVersionsDAL.ApproveVersion(articleId, date, value);
        }

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid articleId, int number, bool value = true)
        {
            return _articleVersionsDAL.ApproveVersion(articleId, number, value);
        }

        /// <summary>
        /// Returns a number of versions for article.
        /// </summary>
        /// <param name="articleId">Artcle to count</param>
        /// <returns>Number of versions</returns>
        public int VersionsCount(Guid articleId)
        {
            return GetAllVersionOfArticle(articleId).Count();
        }

        /// <summary>
        /// Checks for login in database.
        /// </summary>
        /// <param name="shortUrl">Checked short url</param>
        /// <returns>Whether heading is exist or not</returns>
        public bool IsShortUrlExist(string shortUrl)
        {
            return _articlesDAL.GetArticle(shortUrl) == null;
        }

        /// <summary>
        /// Gets a random article from database.
        /// </summary>
        /// <returns></returns>
        public ArticleBDO GetRandomArticle()
        {
            var article = _articlesDAL.GetRandomArticle();
            return CreateArticleBDO(_articleVersionsDAL.GetLastVersion(article.Id));
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        public int GetNumberOfVersion(Guid versionId)
        {
            return _articleVersionsDAL.GetNumberOfVersion(versionId);
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        public int GetNumberOfVersion(Guid articleId, DateTime date)
        {
            return _articleVersionsDAL.GetNumberOfVersion(articleId, date);
        }

        private ArticleBDO CreateArticleBDO(ArticleVersionDTO versionDTO)
        {
            var articleDTO = _articlesDAL.GetArticle(versionDTO.ArticleId);
            var contentDTO = _articleContentsDAL.GetContent(versionDTO.ContentId);
            return new ArticleBDO()
            {
                Id = articleDTO.Id,
                ShortUrl = articleDTO.ShortUrl,
                AuthorId = articleDTO.AuthorId,
                CreationDate = articleDTO.CreationDate,

                VersionId = versionDTO.Id,
                LastEditDate = versionDTO.LastEditDate,
                EditionAuthorId = versionDTO.EditionAuthorId,
                IsApproved = versionDTO.IsApproved,

                ContentId = contentDTO.Id,
                Heading = contentDTO.Heading,
                Definition = contentDTO.Definition,
                Text = contentDTO.Text,
                ImageId = contentDTO.ImageId
            };
        }

        private ArticleDTO CreateArticleDTO(ArticleBDO articleBDO)
        {
            return new ArticleDTO()
            {
                Id = articleBDO.Id,
                ShortUrl = articleBDO.ShortUrl,
                AuthorId = articleBDO.AuthorId,
                CreationDate = articleBDO.CreationDate
            };
        }

        private ArticleContentDTO CreateArticleContentDTO(ArticleBDO articleBDO)
        {
            var contentDTO = new ArticleContentDTO()
            {
                Heading = articleBDO.Heading,
                Definition = articleBDO.Definition,
                Text = articleBDO.Text,
                ImageId = articleBDO.ImageId
            };
            contentDTO.Id = articleBDO.ContentId == default(Guid) ? Guid.NewGuid() : articleBDO.ContentId;
            return contentDTO;
        }

        private ArticleVersionDTO CreateArticleVersionDTO(ArticleBDO articleBDO, ArticleContentDTO contentDTO)
        {
            var versionDTO = new ArticleVersionDTO()
            {
                ArticleId = articleBDO.Id,
                ContentId = contentDTO.Id,
                LastEditDate = articleBDO.LastEditDate,
                EditionAuthorId = articleBDO.EditionAuthorId,
                IsApproved = articleBDO.IsApproved
            };
            versionDTO.Id = articleBDO.VersionId == default(Guid) ? Guid.NewGuid() : articleBDO.VersionId;
            return versionDTO;
        }

	    /// <summary>
	    /// Performs search in all latest versions' headers, then texts 
	    /// and returns versions that apply the query
	    /// </summary>
	    /// <param name="query">text to search in article text</param>
	    /// <returns>ArticleVersions DTOs</returns>
	    public IEnumerable<ArticleBDO> SearchArticles(string query)
	    {
			if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
				throw new ArgumentNullException(nameof(query), "Search query should be more than 2 character long");

			/* Wildcards support */
			query = query.Replace("[", "[[]").Replace("%", "[%]").Replace("‌​_", "[_]"); // escaping sql wildcards
			query = query.Replace("?", "_").Replace("*", "%"); //converting

			var found = new List<ArticleBDO>();

			/* Headers */
			found.AddRange(_articleVersionsDAL.SearchInHeadings(query).Select(CreateArticleBDO));

			/* Definitions */
			found.AddRange(_articleVersionsDAL.SearchInDefinitions(query).Select(CreateArticleBDO));

			/* Text */
			found.AddRange(_articleVersionsDAL.SearchInText(query).Select(CreateArticleBDO));

			/* GUID */
			var id = Guid.Empty;
			if (Guid.TryParse(query, out id))
			{
				// Article Id
				try { found.Add(GetArticle(id)); }
				catch (EntryNotFoundException) { } // but throws other exceptions

				// Version Id
				try { found.Add(GetVersionOfArticle(id)); }
				catch (EntryNotFoundException) { } // but throws other exceptions
			}

			return found.Distinct(); // remove duplicates
		}
    }
}
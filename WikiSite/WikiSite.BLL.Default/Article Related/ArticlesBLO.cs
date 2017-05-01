using System;
using System.Collections.Generic;
using System.Linq;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
    public class ArticleBLO : IArticlesBLL
    {
        private IArticlesDAL _articlesDAL;
        private IArticleVersionsDAL _articleVersionsDAL;
        private IArticleContentsDAL _articleContentsDAL;
        private DateTime _defaultSqlDaeDateTime = new DateTime(1753, 1, 1);

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
        /// Adds Article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        /// <returns></returns>
        public bool AddArticle(ArticleBDO article)
        {
            CheckThrowDTO(article);
            var articleDTO = CreateArticleDTO(article);
            var contentDTO = CreateArticleContentDTO(article);
            var versionDTO = CreateArticleVersionDTO(article, contentDTO);
            return _articlesDAL.AddArticle(articleDTO) && _articleContentsDAL.AddContent(contentDTO) &&
                   _articleVersionsDAL.AddVersion(versionDTO);
        }

        /// <summary>
        /// Removes Article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        /// <returns></returns>
        public bool RemoveArticle(Guid articleId)
        {
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
        /// Adds new version of article to database.
        /// </summary>
        /// <param name="article">Article BDO</param>
        public bool UpdateArticle(ArticleBDO article)
        {
            CheckThrowDTO(article);
            var contentDTO = CreateArticleContentDTO(article);
            var versionDTO = CreateArticleVersionDTO(article, contentDTO);
            return _articleContentsDAL.AddContent(contentDTO) && _articleVersionsDAL.AddVersion(versionDTO);
        }

        /// <summary>
        /// Gets a certain last edit version of article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of last edit version of a article</returns>
        public ArticleBDO GetLastVersionOftArticle(Guid articleId)
        {
            if (articleId == Guid.Empty) throw new ArgumentNullException(nameof(articleId), "Id is empty.");
            return CreateArticleBDO(_articleVersionsDAL.GetLastVersion(articleId));
        }

        /// <summary>
        /// Gets a certain last approved version of article from a database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of last approved version of a article</returns>
        public ArticleBDO GetLastApprovedVersionOfArticle(Guid articleId)
        {
            if (articleId == Guid.Empty) throw new ArgumentNullException(nameof(articleId), "Id is empty.");
            return CreateArticleBDO(_articleVersionsDAL.GetLastApprovedVersion(articleId));
        }

        /// <summary>
        /// Gets a certain version of article from a database.
        /// </summary>
        /// <param name="articleVersionId">GUID of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleVersionId)
        {
            if (articleVersionId == Guid.Empty)
                throw new ArgumentNullException(nameof(articleVersionId), "Id is empty.");
            return CreateArticleBDO(_articleVersionsDAL.GetVersion(articleVersionId));
        }

        /// <summary>
        /// Gets a certain version of article from a database.
        /// </summary>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleId, DateTime date)
        {
            if (articleId == Guid.Empty) throw new ArgumentNullException(nameof(articleId), "Id is empty.");
            if ((date == default(DateTime)) || (DateTime.Compare(date, _defaultSqlDaeDateTime) <= 0))
                throw new ArgumentNullException(nameof(date),
                    $"Date is less than or equal to default value ({_defaultSqlDaeDateTime.ToShortDateString()}).");
            return CreateArticleBDO(_articleVersionsDAL.GetVersion(articleId, date));
        }

        /// <summary>
        /// Gets a certain version of article from a database.
        /// </summary>
        /// <remarks>
        /// Number is calculated by date.
        /// </remarks>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>Article BDO</returns>
        public ArticleBDO GetVersionOfArticle(Guid articleId, int number)
        {
            if (articleId == Guid.Empty) throw new ArgumentNullException(nameof(articleId), "Id is empty.");
            if (number == default(int)) throw new ArgumentNullException(nameof(number), "Number is the default value.");
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
                versionDTOs.Add(_articleVersionsDAL.GetLastApprovedVersion(article.Id));
            }
            if (articleDTOs.Count != versionDTOs.Count)
                throw new ApplicationException("Something has gone wrong with getting all articles from database.");
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
        /// Approves a certain version of article from a database.
        /// </summary>
        /// <param name="versionId">>GUID of version to approve</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid versionId)
        {
            return _articleVersionsDAL.ApproveVersion(versionId);
        }

        /// <summary>
        /// Approves a certain article's version by date in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid articleId, DateTime date)
        {
            return _articleVersionsDAL.ApproveVersion(articleId, date);
        }

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <returns></returns>
        public bool ApproveVersionOfArticle(Guid articleId, int number)
        {
            return _articleVersionsDAL.ApproveVersion(articleId, number);
        }

        private void CheckThrowDTO(ArticleBDO bdo)
        {
            if (bdo == null) throw new ArgumentNullException(nameof(bdo), "Article BDO is null.");
            if (bdo.Id == Guid.Empty) throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain ID.");
            if (string.IsNullOrWhiteSpace(bdo.ShortUrl))
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain a short url.");
            if (bdo.AuthorId == Guid.Empty)
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain author id.");
            if (string.IsNullOrWhiteSpace(bdo.Heading))
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain a heading.");
            if ((bdo.CreationDate == default(DateTime)) ||
                (DateTime.Compare(bdo.CreationDate, _defaultSqlDaeDateTime) <= 0))
                throw new ArgumentNullException(nameof(bdo),
                    $"Creation date of Article BDO is less than or equal to default value ({_defaultSqlDaeDateTime.ToShortDateString()}).");
            if ((bdo.LastEditDate == default(DateTime)) ||
                (DateTime.Compare(bdo.LastEditDate, _defaultSqlDaeDateTime) <= 0))
                throw new ArgumentNullException(nameof(bdo),
                    $"Last edit date of Article BDO is less than or equal to default value ({_defaultSqlDaeDateTime.ToShortDateString()}).");
            if (bdo.EditionAuthorId == Guid.Empty)
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain edition author id.");
            if (string.IsNullOrWhiteSpace(bdo.Definition))
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain a definition.");
            if (string.IsNullOrWhiteSpace(bdo.Text))
                throw new ArgumentNullException(nameof(bdo), "Article BDO doesn't contain a text.");
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
                Heading = articleDTO.Heading,
                CreationDate = articleDTO.CreationDate,

                LastEditDate = versionDTO.LastEditDate,
                EditionAuthorId = versionDTO.EditionAuthorId,
                IsApproved = versionDTO.IsApproved,

                Definition = contentDTO.Definition,
                Text = contentDTO.Text
            };
        }

        private ArticleDTO CreateArticleDTO(ArticleBDO articleBDO)
        {
            return new ArticleDTO()
            {
                Id = articleBDO.Id,
                ShortUrl = articleBDO.ShortUrl,
                AuthorId = articleBDO.AuthorId,
                Heading = articleBDO.Heading,
                CreationDate = articleBDO.CreationDate
            };
        }

        private ArticleContentDTO CreateArticleContentDTO(ArticleBDO articleBDO)
        {
            return new ArticleContentDTO()
            {
                Id = Guid.NewGuid(),
                Definition = articleBDO.Definition,
                Text = articleBDO.Text
            };
        }

        private ArticleVersionDTO CreateArticleVersionDTO(ArticleBDO articleBDO, ArticleContentDTO contentDTO)
        {
            return new ArticleVersionDTO()
            {
                Id = Guid.NewGuid(),
                ArticleId = articleBDO.Id,
                ContentId = contentDTO.Id,
                LastEditDate = articleBDO.LastEditDate,
                EditionAuthorId = articleBDO.EditionAuthorId,
                IsApproved = articleBDO.IsApproved
            };
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Entities;
using WikiSite.Caretakers;
using AutoMapper;

namespace WikiSite.PL.ASP.Models
{
    public class ArticleVM
    {
        #region Instance

        private Guid _id;
        private Guid _authorId;
        private string _heading;
        private string _shortUrl;
        private DateTime _creationDate;
        private DateTime _lastEditDate;
        private Guid _editionAuthorId;
        private string _definition;
        private string _text;
        private Guid _imageId;
        private Guid _versionId;
        private Guid _contentId;

        /// <summary>
        /// ONLY for model binder
        /// </summary>
        public ArticleVM()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            LastEditDate = DateTime.Now;
        }

        public ArticleVM(string heading, Guid authorId, string definition, string text, bool isApproved = false)
        {
            Id = Guid.NewGuid();
            Heading = heading;
            AuthorId = authorId;
            CreationDate = DateTime.Now;
            EditionAuthorId = authorId;
            LastEditDate = CreationDate;
            Definition = definition;
            Text = text;
            IsApproved = isApproved;
        }

        public ArticleVM(Guid id, string heading, Guid authorId, string definition, string text, bool isApproved = false)
        {
            Id = id;
            Heading = heading;
            AuthorId = authorId;
            CreationDate = DateTime.Now;
            EditionAuthorId = authorId;
            LastEditDate = CreationDate;
            Definition = definition;
            Text = text;
            IsApproved = isApproved;
        }

        public ArticleVM(Guid id, string heading, Guid authorId, DateTime creationDate, Guid editionAuthorId, DateTime lastEditDate, string definition, string text, bool isApproved = false)
        {
            Id = id;
            Heading = heading;
            AuthorId = authorId;
            CreationDate = creationDate;
            EditionAuthorId = editionAuthorId;
            LastEditDate = lastEditDate;
            Definition = definition;
            Text = text;
            IsApproved = isApproved;
        }

        public Guid Id
        {
            get { return _id; }
            set
            {
                ErrorGuard.Check(value);
                _id = value;
            }
        }

        public Guid AuthorId
        {
            get { return _authorId; }
            set
            {
                ErrorGuard.Check(value);
                _authorId = value;
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        [MinLength(1, ErrorMessage = "Минимальное количество символов - 5")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов - 50")]
        [Remote("IsHeadingExist", "Article", AreaReference.UseRoot, ErrorMessage = "Такой заголовок уже существует. Воспользуйтесь редактированием.")]
        public string Heading
        {
            get { return _heading; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _heading = value;
                ShortUrl = HttpUtility.UrlEncode(value.ToLower());
            }
        }

        public string ShortUrl {
            get { return HttpUtility.UrlDecode(_shortUrl); }
            set { _shortUrl = value; }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                ErrorGuard.Check(value); 
                _creationDate = value;
            }
        }
        public DateTime LastEditDate {
            get { return _lastEditDate; }
            set
            {
                ErrorGuard.Check(value);
                _lastEditDate = value;
            }
        }
        public Guid EditionAuthorId {
            get { return _editionAuthorId; }
            set
            {
                ErrorGuard.Check(value);
                _editionAuthorId = value;
            }
        }
        public bool IsApproved { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(5, ErrorMessage = "Минимальное количество символов - 5")]
        [MaxLength(300, ErrorMessage = "Максимальное количество символов - 300")]
        [Display(Name = "Краткое описание")]
        public string Definition
        {
            get { return _definition; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _definition = value;
            }
        }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(5, ErrorMessage = "Минимальное количество символов - 5")]
        [Display(Name = "Содержание")]
        public string Text {
            get { return _text; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _text = value;
            }
        }

        [Display(Name = "Картинка")]
        public Guid ImageId
        {
            get { return _imageId; }
            set
            {
                ErrorGuard.Check(value);
                _imageId = value;
            }
        }

        public Guid VersionId {
            get { return _versionId; }
            set
            {
                ErrorGuard.Check(value);
                _versionId = value;
            }
        }

        public Guid ContentId
        {
            get { return _contentId; }
            set
            {
                ErrorGuard.Check(value);
                _contentId = value;
            }
        }
        #endregion

        #region Static

        private static IArticlesBLL _bll;

        static ArticleVM()
        {
            _bll = Provider.ArticlesBLO;
        }

        /// <summary>
        /// Adds new article to database.
        /// </summary>
        /// <param name="article">Article VM</param>
        /// <returns></returns>
        public static bool AddArticle(ArticleVM article)
        {
            return _bll.AddArticle(Mapper.Map<ArticleBDO>(article));
        }

        /// <summary>
        /// Removes article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to delete</param>
        /// <returns></returns>
        public static bool RemoveArticle(Guid articleId)
        {
            return _bll.RemoveArticle(articleId);
        }

        /// <summary>
        /// Adds new NOT approved version of article to database.
        /// </summary>
        /// <param name="article">Article VM</param>
        public static bool UpdateArticle(ArticleVM article)
        {
            return _bll.UpdateArticle(Mapper.Map<ArticleBDO>(article));
        }

        /// <summary>
        /// Returns article in last edit version from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        public static ArticleVM GetArticle(Guid articleId)
        {
            return Mapper.Map<ArticleVM>(_bll.GetArticle(articleId));
        }

        /// <summary>
        /// Returns article in last edit version from database.
        /// </summary>
        /// <param name="shortUrl">Short URL of article to get</param>
        /// <returns>BTO of article in last edit version</returns>
        public static ArticleVM GetArticle(string shortUrl)
        {
            return Mapper.Map<ArticleVM>(_bll.GetArticle(shortUrl));
        }

        /// <summary>
        /// Returns a certain last edit version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>VM of last edit version of a article</returns>
        public static ArticleVM GetLastVersionOftArticle(Guid articleId)
        {
            return Mapper.Map<ArticleVM>(_bll.GetLastVersionOftArticle(articleId));
        }

        /// <summary>
        /// Returns a certain last approved version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>VM of last approved version of a article</returns>
        public static ArticleVM GetLastApprovedVersionOfArticle(Guid articleId)
        {
            return Mapper.Map<ArticleVM>(_bll.GetLastApprovedVersionOfArticle(articleId));
        }

        /// <summary>
        /// Get a certain version of article from database.
        /// </summary>
        /// <param name="articleVersionId">GUID of version of article to get</param>
        /// <returns>Article VM</returns>
        public static ArticleVM GetVersionOfArticle(Guid articleVersionId)
        {
            return Mapper.Map<ArticleVM>(_bll.GetVersionOfArticle(articleVersionId));
        }

        /// <summary>
        /// Returns a certain version of article from database.
        /// </summary>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        /// <returns>Article VM</returns>
        public static ArticleVM GetVersionOfArticle(Guid articleId, DateTime date)
        {
            return Mapper.Map<ArticleVM>(_bll.GetVersionOfArticle(articleId, date));
        }

        /// <summary>
        /// Returns a certain version of article from database.
        /// </summary>
        /// <remarks>
        /// Number is calculated by date.
        /// </remarks>
        /// <param name="articleId">GIUD of article to get</param>
        /// <param name="number">Number of version of article to get</param>
        /// <returns>Article VM</returns>
        public static ArticleVM GetVersionOfArticle(Guid articleId, int number)
        {
            return Mapper.Map<ArticleVM>(_bll.GetVersionOfArticle(articleId, number));
        }

        /// <summary>
        /// Returns all articles form database.
        /// </summary>
        /// <returns>Articles' VMs</returns>
        public static IEnumerable<ArticleVM> GetAllArticles()
        {
            return _bll.GetAllArticles().Select(Mapper.Map<ArticleVM>);
        }

        /// <summary>
        /// Returns all articles form database, which created by author.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' VMs</returns>
        public static IEnumerable<ArticleVM> GetAllArticles(Guid authorId)
        {
            return _bll.GetAllArticles(authorId).Select(Mapper.Map<ArticleVM>);
        }

        /// <summary>
        /// Returns all version of article from database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <returns>Articles' VMs</returns>
        public static IEnumerable<ArticleVM> GetAllVersionOfArticle(Guid articleId)
        {
            return _bll.GetAllVersionOfArticle(articleId).Select(Mapper.Map<ArticleVM>);
        }

        /// <summary>
        /// Returns all version, which created by author, from database.
        /// </summary>
        /// <param name="authorId">GUID of author to get</param>
        /// <returns>Articles' VMs</returns>
        public static IEnumerable<ArticleVM> GetAllVersionByAuthor(Guid authorId)
        {
            return _bll.GetAllVersionByAuthor(authorId).Select(Mapper.Map<ArticleVM>);
        }

        /// <summary>
        /// Approves a certain version of article from database.
        /// </summary>
        /// <param name="versionId">>GUID of version to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public static bool ApproveVersionOfArticle(Guid versionId, bool value = true)
        {
            return _bll.ApproveVersionOfArticle(versionId, value);
        }

        /// <summary>
        /// Approves a certain article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="date">DateTime of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public static bool ApproveVersionOfArticle(Guid articleId, DateTime date, bool value = true)
        {
            return _bll.ApproveVersionOfArticle(articleId, date, value);
        }

        /// <summary>
        /// Approves a certain article's version by number in sorted database by time.
        /// </summary>
        /// <param name="articleId">GUID of article to approve</param>
        /// <param name="number">Number of version of article to approve</param>
        /// <param name="value">Feature for disapprove, default is true</param>
        /// <returns></returns>
        public static bool ApproveVersionOfArticle(Guid articleId, int number, bool value = true)
        {
            return _bll.ApproveVersionOfArticle(articleId, number, value);
        }

        /// <summary>
        /// Returns a number of versions for article.
        /// </summary>
        /// <param name="articleId">Artcle to count</param>
        /// <returns>Number of versions</returns>
        public static int VersionsCount(Guid articleId)
        {
            return _bll.VersionsCount(articleId);
        }

        /// <summary>
        /// Checks for login in database.
        /// </summary>
        /// <param name="shortUrl">Checked short url</param>
        /// <returns>Whether heading is exist or not</returns>
        public static bool IsShortUrlExist(string shortUrl)
        {
            return _bll.IsShortUrlExist(shortUrl);
        }
        
        /// <summary>
        /// Returns a random article from database.
        /// </summary>
        /// <returns></returns>
        public static ArticleVM GetRandomArticle()
        {
            return Mapper.Map<ArticleVM>(_bll.GetRandomArticle());
        }

        /// <summary>
        /// Returns a guide article from database.
        /// </summary>
        /// <returns></returns>
        public static ArticleVM GetGuideArticle()
        {
            return Mapper.Map<ArticleVM>(_bll.GetGuideArticle());
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="versionId">GUID of version to get</param>
        public static int GetNumberOfVersion(Guid versionId)
        {
            return _bll.GetNumberOfVersion(versionId);
        }

        /// <summary>
        /// Returns a number of article's version by date time in database.
        /// </summary>
        /// <param name="articleId">GUID of article to get</param>
        /// <param name="date">DateTime of version of article to get</param>
        public static int GetNumberOfVersion(Guid articleId, DateTime date)
        {
            return _bll.GetNumberOfVersion(articleId, date);
        }

	    /// <summary>
	    /// Performs search in all latest versions' headers, then texts 
	    /// and returns versions that apply the query
	    /// </summary>
	    /// <param name="query">text to search in article text</param>
	    /// <returns>Article VMs</returns>
	    public static IEnumerable<ArticleVM> SearchArticles(string query)
	    {
		    return Mapper.Map<IEnumerable<ArticleVM>>(_bll.SearchArticles(query));
	    }

		#endregion
	}
}
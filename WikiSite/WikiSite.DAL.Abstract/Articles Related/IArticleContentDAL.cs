using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiSite.Entities;

namespace WikiSite.DAL.Abstract
{
    public interface IArticleContentDAL
    {
        /// <summary>
		/// Adds article's content to a database.
		/// </summary>
		/// <param name="content">Article's content DTO</param>
        bool AddContent(ArticleContentDTO content);
        /// <summary>
        /// Removes article's content from a database.
        /// </summary>
        /// <param name="contentId">GUID of article's content to delete</param>
        bool RemoveContent(Guid contentId);
        /// <summary>
        /// Gets article's content from a database.
        /// </summary>
        /// <param name="contentId">GUID of article's content to get</param>
        /// <returns>DTO of a article's content</returns>
        ArticleContentDTO GetContent(Guid contentId);
        /// <summary>
		/// Updates article's content in a database.
		/// </summary>
		/// <param name="content">Article's content DTO with the same ID and new data</param>
        bool UpdateContent(ArticleContentDTO content);
    }
}

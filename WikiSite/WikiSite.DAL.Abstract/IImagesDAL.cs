using System;
using System.Web;

namespace WikiSite.DAL.Abstract
{
    public interface IImagesDAL
    {
        /// <summary>
        /// Adds image to database.
        /// </summary>
        /// <param name="image">Image to add</param>
        /// <returns></returns>
        bool AddImage(HttpPostedFileBase image);

        /// <summary>
        /// Removes image from database.
        /// </summary>
        /// <param name="id">Id of image to remove</param>
        /// <returns></returns>
        bool Remove(Guid id);
    }
}

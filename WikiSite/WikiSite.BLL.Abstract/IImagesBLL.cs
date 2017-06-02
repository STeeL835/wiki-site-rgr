using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiSite.Entities;

namespace WikiSite.BLL.Abstract
{
    public interface IImagesBLL
    {
        /// <summary>
        /// Adds image to database.
        /// </summary>
        /// <param name="image">Image DTO</param>
        /// <returns></returns>
        bool AddImage(ImageDTO image);

        /// <summary>
        /// Gets image from database.
        /// </summary>
        /// <param name="id">Guid of image</param>
        /// <returns></returns>
        ImageDTO GetImage(Guid id);

        /// <summary>
        /// Removes image from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveImage(Guid id);

        /// <summary>
        /// Gets all images from database;
        /// </summary>
        /// <returns></returns>
        IEnumerable<ImageDTO> GetAllImage();
    }
}

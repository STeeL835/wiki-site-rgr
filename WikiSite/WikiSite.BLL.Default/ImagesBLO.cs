using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WikiSite.BLL.Abstract;
using WikiSite.DAL.Abstract;
using WikiSite.Entities;

namespace WikiSite.BLL.Default
{
    public class ImagesBLO : IImagesBLL
    {
        private IImagesDAL _imagesDAL;

        public ImagesBLO(IImagesDAL imagesDAL)
        {
            if (imagesDAL == null)
                throw new ArgumentNullException(nameof(imagesDAL), "Image DAL instance is null.");
            _imagesDAL = imagesDAL;
        }

        /// <summary>
        /// Adds image to database.
        /// </summary>
        /// <param name="image">Image DTO</param>
        /// <returns></returns>
        public bool AddImage(ImageDTO image)
        {
            return _imagesDAL.AddImage(image);
        }

        /// <summary>
        /// Gets image from database.
        /// </summary>
        /// <param name="id">Guid of image</param>
        /// <returns></returns>
        public ImageDTO GetImage(Guid id)
        {
            return _imagesDAL.GetImage(id);
        }

        /// <summary>
        /// Removes image from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveImage(Guid id)
        {
            return _imagesDAL.RemoveImage(id);
        }

        /// <summary>
        /// Gets all images from database;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageDTO> GetAllImage()
        {
            return _imagesDAL.GetAllImage();
        }
    }
}

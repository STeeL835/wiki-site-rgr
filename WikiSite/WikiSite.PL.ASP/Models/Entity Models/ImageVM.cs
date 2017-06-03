using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using AutoMapper;
using WikiSite.BLL.Abstract;
using WikiSite.Caretakers;
using WikiSite.DI.Provider;
using WikiSite.Entities;

namespace WikiSite.PL.ASP.Models
{
    public class ImageVM
    {
        #region Instance
        private Guid _id;
        private string _type;
        private byte[] _data;
        public Guid Id {
            get { return _id; }
            set
            {
                ErrorGuard.Check(value);
                _id = value;
            }
        }
        public byte[] Data {
            get { return _data; }
            set
            {
                ErrorGuard.Check(value);
                _data = value;
            }
        }
        public string Type {
            get { return _type; }
            set
            {
                ErrorGuard.Check(value);
                _type = value;
            }
        }

        public ImageVM(byte[] data, string type)
        {
            Id = Guid.NewGuid();
            Data = data;
            Type = type;
        }

        public ImageVM()
        {

        }

        public ImageVM(Guid id, byte[] data, string type)
        {
            Id = id;
            Data = data;
            Type = type;
        }
        #endregion

        #region Static

        private static IImagesBLL _bll;
        public static Guid DefaultImageGuid {
            get { return Guid.Parse("4f2d22b0-4af3-46be-a3c4-ae74b5b5d1ca");}
        }

        static ImageVM()
        {
            _bll = Provider.ImagesBLO;
        }
        /// <summary>
        /// Adds image to database.
        /// </summary>
        /// <param name="image">Image DTO</param>
        /// <returns></returns>
        public static bool AddImage(ImageVM image)
        {
            return _bll.AddImage(Mapper.Map<ImageDTO>(image));
        }

        /// <summary>
        /// Gets image from database.
        /// </summary>
        /// <param name="id">Guid of image</param>
        /// <returns></returns>
        public static ImageVM GetImage(Guid id)
        {
            return Mapper.Map<ImageVM>(_bll.GetImage(id));
        }

        /// <summary>
        /// Gets default image from database;
        /// </summary>
        /// <returns></returns>
        public static ImageVM GetDefaultImage()
        {
            return GetImage(DefaultImageGuid);
        }

        /// <summary>
        /// Removes image from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool RemoveImage(Guid id)
        {
            return _bll.RemoveImage(id);
        }

        /// <summary>
        /// Gets all images from database;
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<ImageVM> GetAllImage()
        {
            return _bll.GetAllImage().Select(Mapper.Map<ImageVM>);
        }
        #endregion
    }
}
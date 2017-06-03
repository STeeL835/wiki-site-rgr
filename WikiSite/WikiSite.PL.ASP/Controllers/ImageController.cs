using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class ImageController : Controller
    {
        public static Guid Add(HttpPostedFileBase file)
        {
            if (file == null) return ImageVM.DefaultImageGuid;
            var tempData = new byte[file.ContentLength];
            using (BinaryReader reader = new BinaryReader(file.InputStream))
            {
                for (int i = 0; i < tempData.Length; i++)
                {
                    tempData[i] = reader.ReadByte();
                }
            }
            var image = new ImageVM(tempData, file.ContentType);
            ImageVM.AddImage(image);
            return image.Id;
        }

        public FileResult Get(Guid id)
        {
            var image = id == default(Guid) ? ImageVM.GetDefaultImage() : ImageVM.GetImage(id);
            return File(image.Data, image.Type);
        }

        public FileResult GetByUrl(string url, int number = 0)
        {
            if(url != null)
                if (number != 0)
                {
                    return Get(ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(url).Id, number).ImageId);
                }
                else
                {
                    return Get(ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id).ImageId);
                }
            else
            {
                return Get(ImageVM.DefaultImageGuid);
            }
        }

        public FileResult GetResized(int width, int height, Guid id)
        {
            var imageVM = id == default(Guid) ? ImageVM.GetDefaultImage() : ImageVM.GetImage(id);
            Image image;
            using (var ms = new MemoryStream(imageVM.Data))
            {
                image = new Bitmap(ms);
            }

            var resizedImage = new Bitmap(image.Resize(width, height, true));
            using (var streak = new MemoryStream())
            {
                resizedImage.Save(streak, ImageFormat.Png);
                return File(streak.ToArray(), "image/png");
            }
        }

        public FileResult GetResizedByUrl(int width, int height, string url, int number = 0)
        {
            if (url != null)
                if (number != 0)
                {
                    return GetResized(width, height, ArticleVM.GetVersionOfArticle(ArticleVM.GetArticle(url).Id, number).ImageId);
                }
                else
                {
                    return GetResized(width, height, ArticleVM.GetLastApprovedVersionOfArticle(ArticleVM.GetArticle(url).Id).ImageId);
                }
            else
            {
                return GetResized(width, height, ImageVM.DefaultImageGuid);
            }
        }
    }
}
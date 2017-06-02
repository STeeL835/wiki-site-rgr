using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikiSite.PL.ASP.Classes;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP.Controllers
{
    public class ImageController : Controller
    {
        public static Guid AddImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
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
            return ImageVM.DefaultImageGuid;
        }

        public ActionResult GetResizedImage(string url, int width, int height)
        {
            ImageVM imageSrc;
            if (url != null)
            {
                imageSrc = ImageVM.GetImage(ArticleVM.GetArticle(url).ImageId);
            }
            else
            {
                imageSrc = ImageVM.GetDefaultImage();
            }
            
            Image image;
            using (var ms = new MemoryStream(imageSrc.Data))
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
    }
}
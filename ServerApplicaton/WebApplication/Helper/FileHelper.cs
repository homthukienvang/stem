using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Extensions;

namespace WebApplication.Helper
{
    public static class UploadHelper
    {
        /// <summary>
        /// Uploads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static string UploadImage(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            string image = "";
            if (file != null && file.ContentLength > 0)
            {
                string folder = server.MapPath("~/Images");

                bool isExists = Directory.Exists(folder);
                if (!isExists)
                    Directory.CreateDirectory(folder);
                string ext = Path.GetExtension(file.FileName);
                var filename = "img_" + DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss") + ext;
                var path = Path.Combine(folder, filename);
                file.SaveAs(path);
                image = filename;
            }
            return image;
        }
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="filepdf">The filepdf.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static string UploadFile(HttpPostedFileBase filepdf, string folderPath, HttpServerUtilityBase server)
        {
            string image = "";
            if (filepdf != null && filepdf.ContentLength > 0)
            {
                string folder = server.MapPath(folderPath);

                bool isExists = Directory.Exists(folder);
                if (!isExists)
                    Directory.CreateDirectory(folder);
                //string ext = Path.GetExtension(filepdf.FileName);
                var filename = DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss") + filepdf.FileName;
                var path = Path.Combine(folder, filename);
                filepdf.SaveAs(path);
                image = filename;
            }
            return image;
        }
        /// <summary>
        /// Uploads the crop.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static string UploadCropImage(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            string imageUrl = "";
            if (file != null && file.ContentLength > 0)
            {
                string folder = server.MapPath("~/Images");

                var image = WebImage.GetImageFromRequest();
                if (image != null)
                {
                    if (image.Width > 100)
                    {
                        image.Resize(100, ((100 * image.Height) / image.Width));
                    }

                    string ext = Path.GetExtension(image.FileName);
                    var file_name = DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss");
                    image.Save(Path.Combine(folder, "img_" + file_name + ext));
                    imageUrl = "/Images/" + "img_" + file_name + ext;

                    return imageUrl;
                }
            }
            return imageUrl;
        }
        /// <summary>
        /// Uploads the crop and resize.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static string UploadCropAndResizeImage(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            string imageUrl = "";
            if (file != null && file.ContentLength > 0)
            {
                string folder = server.MapPath(GlobalSession.ImageFolder);

                var image = WebImage.GetImageFromRequest();
                if (image != null)
                {
                    if (image.Width > 800)
                    {
                        image.Resize(800, ((800 * image.Height) / image.Width));
                    }

                    // string ext = Path.GetExtension(image.FileName);
                    var file_name = image.FileName;// "img_" + DateTime.UtcNow.ToTimeString() + ext;
                    image.Save(Path.Combine(folder, file_name));
                    //imageUrl = "/Images/" + file_name + ext;
                    var height = image.Height;
                    var width = image.Width;
                    image.Resize(350, ((350 * image.Height) / image.Width));

                    image.Save(Path.Combine(folder + "Thumbs/", file_name));

                    imageUrl = file_name;
                    return imageUrl;
                }
            }
            return imageUrl;
        }

    }
}
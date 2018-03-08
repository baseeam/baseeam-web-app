/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Security;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class PictureController : BaseController
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PictureController(IRepository<Picture> pictureRepository)
        {
            this._pictureRepository = pictureRepository;
        }

        [HttpPost]
        //do not validate request token (XSRF)
        [BaseEamAntiForgery(true)]
        public virtual ActionResult AsyncUpload(long entityId, string entityType)
        {
            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    default:
                        break;
                }
            }

            var picture = new Picture
            {
                EntityId = entityId,
                EntityType = entityType,
                ImageBytes = fileBinary,
                ContentType = contentType,
                Extension = fileExtension,
                Name = fileName
            };

            _pictureRepository.InsertAndCommit(picture);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageSrc = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(picture.ImageBytes))
            }, MimeTypes.TextPlain);
        }

        [HttpPost]
        [BaseEamAntiForgery(true)]
        public ActionResult Delete(long entityId, string entityType)
        {
            var picture = _pictureRepository.GetAll().FirstOrDefault(a => a.EntityId == entityId && a.EntityType == entityType);
            if (picture == null)
                throw new ArgumentException("No attach image found with the specified id");

            _pictureRepository.DeactivateAndCommit(picture);
            return new NullJsonResult();
        }
    }
}
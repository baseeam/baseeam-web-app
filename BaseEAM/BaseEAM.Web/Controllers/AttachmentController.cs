/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class AttachmentController : BaseController
    {
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<EntityAttachment> _entityAttachmentRepository;

        public AttachmentController(IRepository<Attachment> attachmentRepository,
            IRepository<EntityAttachment> entityAttachmentRepository)
        {
            this._attachmentRepository = attachmentRepository;
            this._entityAttachmentRepository = entityAttachmentRepository;
        }

        [HttpPost]
        public ActionResult List(long entityId, string entityType, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _attachmentRepository.GetAll()
                .Where(a => a.EntityAttachments.Any(e => e.EntityId == entityId && e.EntityType == entityType));
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var attachments = new PagedList<Attachment>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = attachments.Select(x => x.ToModel()),
                Total = attachments.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
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

            var attachment = new Attachment
            {
                FileBytes = fileBinary,
                FileSize = fileBinary.Length,
                ContentType = contentType,
                Extension = fileExtension,
                Name = fileName
            };

            attachment.EntityAttachments.Add(new EntityAttachment
            {
                EntityId = entityId,
                EntityType = entityType
            });

            _attachmentRepository.InsertAndCommit(attachment);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                attachmentId = attachment.Id
            }, MimeTypes.TextPlain);
        }

        [HttpPost]
        [BaseEamAntiForgery(true)]
        public ActionResult Delete(long id, long entityId, string entityType)
        {
            var attachment = _attachmentRepository.GetById(id);
            if (attachment == null)
                throw new ArgumentException("No attachment found with the specified id");

            var entityAttachment = attachment.EntityAttachments
                .Where(e => e.EntityId == entityId && e.EntityType == entityType)
                .FirstOrDefault();
            if(entityAttachment != null)
                attachment.EntityAttachments.Remove(entityAttachment);

            _attachmentRepository.UpdateAndCommit(attachment);
            return new NullJsonResult();
        }

        public ActionResult Download(long id)
        {
            var attachment = _attachmentRepository.GetById(id);
            if (attachment == null)
                return Content("No attachment record found with the specified id");

            //use stored data
            if (attachment.FileBytes == null)
                return Content(string.Format("Download data is not available any more. Attachment ID={0}", id));

            string fileName = !String.IsNullOrWhiteSpace(attachment.Name) ? attachment.Name : id.ToString();
            string contentType = !String.IsNullOrWhiteSpace(attachment.ContentType) ? attachment.ContentType : "application/octet-stream";
            return new FileContentResult(attachment.FileBytes, contentType) { FileDownloadName = fileName + attachment.Extension };
        }
    }
}
/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Security;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class UploadFileController : BaseController
    {
        private readonly IRepository<ImportProfile> _importProfileRepository;
        private readonly IDbContext _dbContext;

        public UploadFileController(IRepository<ImportProfile> importProfileRepository,
            IDbContext dbContext)
        {
            this._importProfileRepository = importProfileRepository;
            this._dbContext = dbContext;
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
            var success = false;
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

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var entity = this._dbContext.GetByEntityIdAndType(entityId, entityType);
            if (entityType == EntityType.Report)
            {
                var fileBinary = new byte[stream.Length];
                stream.Read(fileBinary, 0, fileBinary.Length);
                ((Report)entity).TemplateFileName = fileName;
                ((Report)entity).TemplateFileBytes = fileBinary;
                success = true;
            }
            else if(entityType == EntityType.ImportProfile)
            {
                var importProfile = _importProfileRepository.GetById(entityId);
                var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
                var importProfileFolder = string.Format("Profile{0}", importProfile.Id);
                var importProfilePath = Path.Combine(rootPath, importProfileFolder);
                if (Directory.Exists(importProfilePath))
                {
                    var importFileNamePath = Path.Combine(importProfilePath, fileName);
                    success = WriteToFile(stream, importFileNamePath);
                    if (success)
                    {
                        importProfile.ImportFileName = fileName;
                        importProfile.FileTypeId = fileExtension.Contains(ImportFileType.CSV.ToString()) ? (int?)ImportFileType.CSV : (int?)ImportFileType.XLSX;
                        this._dbContext.SaveChanges();
                    }

                }
            }

            this._dbContext.SaveChanges();

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = success,
                downloadUrl = Url.Action("DownloadFile", new { entityId = entityId, entityType = entityType })            
            }, MimeTypes.TextPlain);
        }

        public bool WriteToFile(Stream srcStream, string path)
        {
            if (srcStream == null)
                return false;

            const int BuffSize = 32768;
            bool result = true;
            Stream dstStream = null;
            byte[] buffer = new Byte[BuffSize];

            try
            {
                using (dstStream = System.IO.File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    int len;
                    while ((len = srcStream.Read(buffer, 0, BuffSize)) > 0)
                        dstStream.Write(buffer, 0, len);
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (dstStream != null)
                {
                    dstStream.Close();
                    dstStream.Dispose();
                }
            }

            return (result && System.IO.File.Exists(path));
        }

        public virtual ActionResult DownloadFile(long entityId, string entityType)
        {
            var fileName = "";
            var contentType = "";
            byte[] fileBytes = null;

            var entity = this._dbContext.GetByEntityIdAndType(entityId, entityType);
            if (entityType == EntityType.Report)
            {
                fileName = ((Report)entity).TemplateFileName;
                contentType = MimeTypes.ApplicationOctetStream;
                fileBytes = ((Report)entity).TemplateFileBytes;
            }
            else if(entityType == EntityType.ImportProfile)
            {
                var importProfile = _importProfileRepository.GetById(entityId);
                var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
                var importProfileFolder = string.Format("Profile{0}", importProfile.Id);
                var importProfilePath = Path.Combine(rootPath, importProfileFolder, importProfile.ImportFileName);
                var path = Server.MapPath(importProfilePath);
                if (System.IO.File.Exists(path))
                {
                    contentType = importProfile.FileTypeId == (int?)(ImportFileType.CSV) ? MimeTypes.TextCsv : MimeTypes.TextXlsx;
                    var stream = new FileStream(path, FileMode.Open);
                    var fileBinary = new byte[stream.Length];
                    stream.Read(fileBinary, 0, fileBinary.Length);
                    fileBytes = fileBinary;
                }
            }

            if (entity == null)
                return Content("No download record found with the specified id");

            //use stored data
            if (fileBytes == null)
                return Content(string.Format("Download data is not available any more."));

            return new FileContentResult(fileBytes, contentType) { FileDownloadName = fileName };
        }

        [HttpPost]
        [BaseEamAntiForgery(true)]
        public ActionResult Delete(long entityId, string entityType)
        {
            var entity = this._dbContext.GetByEntityIdAndType(entityId, entityType);
            if (entity == null)
                throw new ArgumentException("No attach file found with the specified id");

            if (entityType == EntityType.Report)
            {
                ((Report)entity).TemplateFileName = null;
                ((Report)entity).TemplateFileBytes = null;
            }
            else if (entityType == EntityType.ImportProfile)
            {
                ((ImportProfile)entity).ImportFileName = null;
            }

            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }
    }
}
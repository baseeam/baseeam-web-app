/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Web.Mvc;
using BaseEAM.Services.Pdf;

namespace BaseEAM.Web.Framework.Pdf
{

    public class PdfPartialViewContent : PdfHtmlContent
    {
        public PdfPartialViewContent(string partialViewName, object model, ControllerContext controllerContext)
            : base(PdfViewContent.ViewToString(partialViewName, null, model, true, controllerContext, false))
        {
        }
    }

}

/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Models;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ILocalizationService _localizationService;

        public CommentController(IRepository<Comment> commentRepository,
            ILocalizationService localizationService)
        {
            this._commentRepository = commentRepository;
            this._localizationService = localizationService;
        }

        [HttpPost]
        public ActionResult Create(CommentModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = model.ToEntity();
                comment.IsNew = false;
                _commentRepository.InsertAndCommit(comment);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { Html = this.RenderPartialViewToString("CommentBox", comment) });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpGet]
        public ActionResult AddCommentView()
        {
            return PartialView("_AddCommentView", new CommentModel());
        }
    }
}
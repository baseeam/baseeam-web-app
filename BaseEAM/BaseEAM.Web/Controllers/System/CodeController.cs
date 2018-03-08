/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework.Filters;

namespace BaseEAM.Web.Controllers
{
    public class CodeController : BaseController
    {
        #region Fields

        private readonly IRepository<Code> _codeRepository;
        private readonly ICodeService _codeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public CodeController(IRepository<Code> codeRepository,
            ICodeService codeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._codeRepository = codeRepository;
            this._localizationService = localizationService;
            this._codeService = codeService;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var codeNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Code.Name",
                DbColumn = "Name, Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(codeNameFilter);

            var codeTypeFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "CodeType",
                ResourceKey = "Code.CodeType",
                DbColumn = "CodeType",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Failure,Cause,Resolution",
                CsvValueList = "Failure,Cause,Resolution",
                IsRequiredField = false
            };
            model.Filters.Add(codeTypeFilter);

            return model;
        }

        #endregion

        #region Codes

        [BaseEamAuthorize(PermissionNames = "System.Code.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.CodeSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.CodeSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.CodeSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            //validate
            var errorFilters = model.Validate(searchValues);
            foreach (var filter in errorFilters)
            {
                ModelState.AddModelError(filter.Name, _localizationService.GetResource(filter.ResourceKey + ".Required"));
            }
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.CodeSearchModel] = model;

                PagedResult<Code> data = _codeService.GetCodes(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => x.ToModel()),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Create")]
        public ActionResult Create()
        {
            var code = new Code { IsNew = true };
            _codeRepository.InsertAndCommit(code);
            return Json(new { Id = code.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Code>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "System.Code.Create,Code.Code.Read,Code.Code.Update")]
        public ActionResult Edit(long id)
        {
            var code = _codeRepository.GetById(id);
            var model = code.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Create,Code.Code.Update")]
        public ActionResult Edit(CodeModel model)
        {
            var code = _codeRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                code = model.ToEntity(code);
                if(model.ParentId > 0)
                {
                    var parentCode = _codeRepository.GetById(model.ParentId);
                    if (parentCode.CodeType == "Failure Group")
                        code.CodeType = "Problem";
                    if (parentCode.CodeType == "Problem")
                        code.CodeType = "Cause";
                    if (parentCode.CodeType == "Cause")
                        code.CodeType = "Resolution";
                }
                else
                {
                    code.CodeType = "Failure Group";
                }                

                //always set IsNew to false when saving
                code.IsNew = false;
                //update attributes
                _codeRepository.Update(code);

                //commit all changes
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var code = _codeRepository.GetById(id);

            if (!_codeService.IsDeactivable(code))
            {
                ModelState.AddModelError("Code", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _codeRepository.DeactivateAndCommit(code);
                //notification
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Code.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var codes = new List<Code>();
            foreach (long id in selectedIds)
            {
                var code = _codeRepository.GetById(id);
                if (code != null)
                {
                    if (!_codeService.IsDeactivable(code))
                    {
                        ModelState.AddModelError("Code", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        codes.Add(code);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var code in codes)
                    _codeRepository.Deactivate(code);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public JsonResult FailureGroupList( string param)
        {
            var items = _codeRepository.GetAll()
                .Where(c => c.CodeType == "Failure Group" && c.Name.Contains(param))
                .OrderBy(c => c.Name)
                .ToList();
            var choices = new List<SelectListItem>();
            foreach (var item in items)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult ProblemList(long parentValue, string param)
        {
            var items = _codeRepository.GetAll()
               .Where(c => c.ParentId == parentValue && c.CodeType == "Problem" && c.Name.Contains(param))
               .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
               .ToList();
       
            //add empty value
            if (items.Count > 0)
            {
                items.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(items);
        }

        [HttpPost]
        public JsonResult CauseList(long parentValue, string param)
        {
            var items = _codeRepository.GetAll()
               .Where(c => c.ParentId == parentValue && c.CodeType == "Cause" && c.Name.Contains(param))
               .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
               .ToList();

            //add empty value
            if (items.Count > 0)
            {
                items.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(items);
        }

        [HttpPost]
        public JsonResult ResolutionList(long parentValue, string param)
        {
            var items = _codeRepository.GetAll()
               .Where(c => c.ParentId == parentValue && c.CodeType == "Resolution" && c.Name.Contains(param))
               .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
               .ToList();

            //add empty value
            if (items.Count > 0)
            {
                items.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(items);
        }
        #endregion

        #region TreeView

        [HttpGet]
        public ActionResult TreeView(string valueFieldId, string textFieldId)
        {
            var model = new TreeViewLookup
            {
                TreeType = "Code",
                ValueFieldId = valueFieldId,
                TextFieldId = textFieldId
            };
            return PartialView("_TreeView", model);
        }

        [HttpPost,]
        public ActionResult TreeLoadChildren(int? id = null, string searchName = "")
        {
            var codes = _codeService.GetAllCodesByParentId(id)
                .Select(x => new
                {
                    id = x.Id,
                    Name = x.Name,
                    hasChildren = _codeService.GetAllCodesByParentId(x.Id).Count > 0,
                    imageUrl = Url.Content("~/Content/images/ico-cube.png")
                });

            return Json(codes);
        }

        #endregion
    }
}
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
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Security;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework.Filters;

namespace BaseEAM.Web.Controllers
{
    public class LanguageController : BaseController
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;        

        #endregion

        #region Constructors

        public LanguageController(IRepository<Language> languageRepository,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._languageRepository = languageRepository;
            this._localizationService = localizationService;
            this._languageService = languageService;
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
            var nameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Language.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(nameFilter);

            var displayOrderFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "DisplayOrder",
                ResourceKey = "Common.DisplayOrder",
                DbColumn = "DisplayOrder",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(displayOrderFilter);

            var createdDateTimeFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "CreatedDateTime",
                ResourceKey = "Common.CreatedDateTime",
                DbColumn = "CreatedDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(createdDateTimeFilter);

            var publishedFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Published",
                ResourceKey = "Language.Published",
                DbColumn = "Published",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Boolean,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "True,False",
                CsvValueList = "True,False",            
                IsRequiredField = false
            };
            model.Filters.Add(publishedFilter);

            var flagImageFileNameFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "FlagImageFileName",
                ResourceKey = "Language.FlagImage",
                DbColumn = "FlagImageFileName",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Language",
                MvcAction = "GetAvailableFlagFileNames"
            };
            model.Filters.Add(flagImageFileNameFilter);

            return model;
        }

        #endregion

        #region Languages

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [BaseEamAuthorize(PermissionNames = "Administration.Language.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.LanguageSearchModel] as SearchModel;
            //If not exist, build search model
            if(model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.LanguageSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable <Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.LanguageSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            //validate
            var errorFilters = model.Validate(searchValues);
            foreach(var filter in errorFilters)
            {
                ModelState.AddModelError(filter.Name, _localizationService.GetResource(filter.ResourceKey + ".Required"));
            }
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.LanguageSearchModel] = model;

                IEnumerable<Language> languages = _languageService.GetLanguages(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = languages.Select(x => x.ToModel()),
                    Total = languages.Count()
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Create")]
        public ActionResult Create()
        {
            var language = new Language { IsNew = true };
            _languageService.InsertLanguage(language);
            return Json(new { Id = language.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            var language = _languageRepository.GetById(id);
            //hard delete
            _languageService.DeleteLanguage(language);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Administration.Language.Create,Administration.Language.Read,Administration.Language.Update")]
        public ActionResult Edit(long id)
        {
            var language = _languageService.GetLanguageById(id);
            var model = language.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Create,Administration.Language.Update")]
        public ActionResult Edit(LanguageModel model)
        {
            var language = _languageRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                //ensure we have at least one published language
                var allLanguages = _languageService.GetAllLanguages();
                if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id &&
                    !model.Published)
                {
                    ModelState.AddModelError("Language", _localizationService.GetResource("Language.PublishedLanguageRequired"));
                    return Json(new { Errors = ModelState.SerializeErrors() });
                }

                language = model.ToEntity(language);
                language.IsNew = false;
                _languageService.UpdateLanguage(language);
                
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
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var language = _languageRepository.GetById(id);

            //ensure we have at least one published language
            var allLanguages = _languageService.GetAllLanguages();
            if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id)
            {
                ModelState.AddModelError("Language", _localizationService.GetResource("Language.PublishedLanguageRequired"));
                
            }

            if(!_languageService.IsDeactivable(language))
            {
                ModelState.AddModelError("Language", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _languageService.Deactivate(language);
                _languageService.UpdateLanguage(language);
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
        [BaseEamAuthorize(PermissionNames = "Administration.Language.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var languages = new List<Language>();
            foreach (long id in selectedIds)
            {
                var language = _languageService.GetLanguageById(id);
                if (language != null)
                {
                    if (!_languageService.IsDeactivable(language))
                    {
                        ModelState.AddModelError("Language", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        languages.Add(language);
                    }
                }
            }

            if(ModelState.IsValid)
            {
                //soft delete
                foreach (var language in languages)
                {
                    _languageService.Deactivate(language);
                    _languageService.UpdateLanguage(language);
                }
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public JsonResult GetAvailableFlagFileNames(string param)
        {
            var flagNames = Directory
                .EnumerateFiles(CommonHelper.MapPath("~/Content/Images/flags/"), "*.png", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .ToList();

            var availableFlagFileNames = flagNames.Select(flagName => new SelectListItem
            {
                Text = flagName,
                Value = flagName
            }).ToList();

            return Json(availableFlagFileNames);
        }

        #endregion

        #region Resources

        [HttpPost]
        //do not validate request token (XSRF)
        //for some reasons it does not work with "filtering" support
        [BaseEamAntiForgery(true)]
        public ActionResult Resources(long languageId, DataSourceRequest command, LanguageResourcesListModel model)
        {
            var query = _localizationService
                .GetAllResourceValues(languageId)
                .OrderBy(x => x.Key)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchResourceName))
                query = query.Where(l => l.Key.ToLowerInvariant().Contains(model.SearchResourceName.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(model.SearchResourceValue))
                query = query.Where(l => l.Value.Value.ToLowerInvariant().Contains(model.SearchResourceValue.ToLowerInvariant()));

            var resources = query
                .Select(x => new
                {
                    LanguageId = languageId,
                    Id = x.Value.Key,
                    Name = x.Key,
                    Value = x.Value.Value,
                });

            var gridModel = new DataSourceResult
            {
                Data = resources.PagedForCommand(command),
                Total = resources.Count()
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ResourceUpdate(LanguageResourceModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var resource = _localizationService.GetLocaleStringResourceById(model.Id);
            // if the resourceName changed, ensure it isn't being used by another resource
            if (!resource.ResourceName.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                var res = _localizationService.GetLocaleStringResourceByName(model.Name, model.LanguageId, false);
                if (res != null && res.Id != resource.Id)
                {
                    return Json(new DataSourceResult { Errors = string.Format(_localizationService.GetResource("LanguageResource.NameAlreadyExists"), res.ResourceName) });
                }
            }

            resource.ResourceName = model.Name;
            resource.ResourceValue = model.Value;
            _localizationService.UpdateLocaleStringResource(resource);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ResourceAdd(long languageId, [Bind(Exclude = "Id")] LanguageResourceModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var res = _localizationService.GetLocaleStringResourceByName(model.Name, model.LanguageId, false);
            if (res == null)
            {
                var resource = new LocaleStringResource { LanguageId = languageId };
                resource.ResourceName = model.Name;
                resource.ResourceValue = model.Value;
                _localizationService.InsertLocaleStringResource(resource);
            }
            else
            {
                return Json(new DataSourceResult { Errors = string.Format(_localizationService.GetResource("LanguageResource.NameAlreadyExists"), model.Name) });
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ResourceDelete(long id)
        {
            var resource = _localizationService.GetLocaleStringResourceById(id);
            if (resource == null)
                throw new ArgumentException("No resource found with the specified id");
            _localizationService.DeleteLocaleStringResource(resource);

            return new NullJsonResult();
        }

        #endregion

        #region Export / Import

        public ActionResult ExportXml(long id)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                //No language found with the specified id
                return RedirectToAction("List");

            try
            {
                var xml = _localizationService.ExportResourcesToXml(language);
                return new XmlDownloadResult(xml, "language_pack.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult ImportXml(long id, FormCollection form)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                //No language found with the specified id
                return RedirectToAction("List");

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            try
            {
                var file = Request.Files["importxmlfile"];
                if (file != null && file.ContentLength > 0)
                {
                    using (var sr = new StreamReader(file.InputStream, Encoding.UTF8))
                    {
                        string content = sr.ReadToEnd();
                        _localizationService.ImportResourcesFromXml(language, content);
                    }
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Common.UploadFile"));
                    return RedirectToAction("Edit", new { id = language.Id });
                }

                SuccessNotification(_localizationService.GetResource("Language.Imported"));
                return RedirectToAction("Edit", new { id = language.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = language.Id });
            }

        }

        #endregion
    }
}
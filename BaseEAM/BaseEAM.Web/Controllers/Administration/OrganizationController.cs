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
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Filters;

namespace BaseEAM.Web.Controllers
{
    public class OrganizationController : BaseController
    {
        #region Fields

        private readonly IRepository<Organization> _organizationRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IOrganizationService _organizationService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public OrganizationController(IRepository<Organization> organizationRepository,
            IRepository<Address> addressRepository,
            IRepository<Site> siteRepository,
            IOrganizationService organizationService,
            ISettingService settingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._organizationRepository = organizationRepository;
            this._addressRepository = addressRepository;
            this._siteRepository = siteRepository;
            this._localizationService = localizationService;
            this._organizationService = organizationService;
            this._settingService = settingService;
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
            var organizationNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "OrganizationName",
                ResourceKey = "Organization.Name",
                DbColumn = "Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Organization",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(organizationNameFilter);            

            return model;
        }

        #endregion

        #region Organizations

        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.OrganizationSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.OrganizationSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.OrganizationSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.OrganizationSearchModel] = model;

                PagedResult<Organization> data = _organizationService.GetOrganizations(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create")]
        public ActionResult Create()
        {
            var organization = new Organization { IsNew = true };
            _organizationRepository.InsertAndCommit(organization);
            return Json(new { Id = organization.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Organization>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Read,Administration.Organization.Update")]
        public ActionResult Edit(long id)
        {
            var organization = _organizationRepository.GetById(id);
            var model = organization.ToModel();
            model.InventorySettings = _settingService.LoadSetting<InventorySettings>().ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult Edit(OrganizationModel model)
        {
            var organization = _organizationRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {                
                organization = model.ToEntity(organization);
                //always set IsNew to false when saving
                organization.IsNew = false;
                _organizationRepository.Update(organization);

                // save settings
                _settingService.SaveSetting(new InventorySettings { CostingType = (int?)model.CostingType });

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
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var organization = _organizationRepository.GetById(id);

            if (!_organizationService.IsDeactivable(organization))
            {
                ModelState.AddModelError("Organization", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _organizationRepository.DeactivateAndCommit(organization);
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
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var organizations = new List<Organization>();
            foreach (long id in selectedIds)
            {
                var organization = _organizationRepository.GetById(id);
                if (organization != null)
                {
                    if (!_organizationService.IsDeactivable(organization))
                    {
                        ModelState.AddModelError("Organization", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        organizations.Add(organization);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var organization in organizations)
                    _organizationRepository.Deactivate(organization);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Addresses

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Read,Administration.Organization.Update")]
        public ActionResult AddressList(long organizationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var sites = _organizationRepository.GetById(organizationId).Addresses;
            if(sites.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = sites.AsQueryable<Address>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = sites.Count()
                };

                return Json(gridModel);
            }            
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Read,Administration.Organization.Update")]
        public ActionResult Address(long id)
        {
            var address = _addressRepository.GetById(id);
            var model = address.ToModel();
            var html = this.AddressPanel(model);
            return Json(new { Id = address.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult CreateAddress(long organizationId)
        {
            var address = new Address
            {
                IsNew = true
            };
            _addressRepository.Insert(address);

            var organization = _organizationRepository.GetById(organizationId);
            organization.Addresses.Add(address);

            this._dbContext.SaveChanges();

            var model = new AddressModel();
            model = address.ToModel();
            var html = this.AddressPanel(model);

            return Json(new { Id = address.Id, Html = html });
        }

        [NonAction]
        public string AddressPanel(AddressModel model)
        {
            var html = this.RenderPartialViewToString("_AddressDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult SaveAddress(AddressModel model)
        {
            if (ModelState.IsValid)
            {
                var address = _addressRepository.GetById(model.Id);
                //always set IsNew to false when saving
                address.IsNew = false;
                address = model.ToEntity(address);

                _addressRepository.UpdateAndCommit(address);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult CancelAddress(long id)
        {
            var address = _addressRepository.GetById(id);
            if (address.IsNew == true)
            {
                _addressRepository.DeleteAndCommit(address);
            }
            return new NullJsonResult();            
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult DeleteAddress(long? parentId, long id)
        {
            var organization = _organizationRepository.GetById(parentId);
            var address = _addressRepository.GetById(id);
            //For many-many, need to remove from parent
            organization.Addresses.Remove(address);  
                      
            _addressRepository.UpdateAndCommit(address);

            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Create,Administration.Organization.Update")]
        public ActionResult DeleteSelectedAddresses(long? parentId, long[] selectedIds)
        {
            var organization = _organizationRepository.GetById(parentId);
            foreach (long id in selectedIds)
            {
                var address = _addressRepository.GetById(id);
                //For many-many, need to remove from parent
                organization.Addresses.Remove(address);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Sites

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Read,Administration.Site.Read")]
        public ActionResult SiteList(long organizationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var sites = _organizationRepository.GetById(organizationId).Sites;
            if (sites.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = sites.AsQueryable<Site>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = sites.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Organization.Read,Administration.Site.Create,Administration.Site.Read,Administration.Site.Update")]
        public ActionResult Site(long id)
        {
            var site = _siteRepository.GetById(id);
            var model = site.ToModel();
            var html = this.SitePanel(model);
            return Json(new { Id = site.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Site.Create")]
        public ActionResult CreateSite(long organizationId)
        {
            var site = new Site
            {
                IsNew = true
            };
            _siteRepository.Insert(site);

            var organization = _organizationRepository.GetById(organizationId);
            organization.Sites.Add(site);

            this._dbContext.SaveChanges();

            var model = new SiteModel();
            model = site.ToModel();
            var html = this.SitePanel(model);

            return Json(new { Id = site.Id, Html = html });
        }

        [NonAction]
        public string SitePanel(SiteModel model)
        {
            var html = this.RenderPartialViewToString("_SiteDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Site.Create,Administration.Site.Update")]
        public ActionResult SaveSite(SiteModel model)
        {
            if (ModelState.IsValid)
            {
                var site = _siteRepository.GetById(model.Id);
                //always set IsNew to false when saving
                site.IsNew = false;
                site = model.ToEntity(site);

                _siteRepository.UpdateAndCommit(site);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Site.Create,Administration.Site.Update")]
        public ActionResult CancelSite(long id)
        {
            var site = _siteRepository.GetById(id);
            if (site.IsNew == true)
            {
                _siteRepository.DeleteAndCommit(site);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Site.Delete")]
        public ActionResult DeleteSite(long? parentId, long id)
        {
            var site = _siteRepository.GetById(id);
            //For one-many, delete by set foreign key to null
            site.OrganizationId = null;
            _siteRepository.UpdateAndCommit(site);

            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.Site.Delete")]
        public ActionResult DeleteSelectedSites(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var site = _siteRepository.GetById(id);
                //For one-many, delete by set foreign key to null
                site.OrganizationId = null;
                _siteRepository.Update(site);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}
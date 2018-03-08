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
    public class CompanyController : BaseController
    {
        #region Fields

        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly ICompanyService _companyService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public CompanyController(IRepository<Company> companyRepository,
            IRepository<Address> addressRepository,
            IRepository<Contact> contactRepository,
            ICompanyService companyService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._companyRepository = companyRepository;
            this._addressRepository = addressRepository;
            this._contactRepository = contactRepository;
            this._localizationService = localizationService;
            this._companyService = companyService;
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
            var companyNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Company.Name",
                DbColumn = "Company.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(companyNameFilter);

            var companyCompanyTypeFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "CompanyType",
                ResourceKey = "Company.CompanyType",
                DbColumn = "Company.CompanyTypeId",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Company Type",
                IsRequiredField = false
            };
            model.Filters.Add(companyCompanyTypeFilter);

            return model;
        }

        #endregion

        #region Companies

        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.CompanySearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.CompanySearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.CompanySearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.CompanySearchModel] = model;

                PagedResult<Company> data = _companyService.GetCompanies(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create")]
        public ActionResult Create()
        {
            var company = new Company { IsNew = true };
            _companyRepository.InsertAndCommit(company);
            return Json(new { Id = company.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Company>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Read,Purchasing.Company.Update")]
        public ActionResult Edit(long id)
        {
            var company = _companyRepository.GetById(id);
            var model = company.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult Edit(CompanyModel model)
        {
            var company = _companyRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                company = model.ToEntity(company);
                //always set IsNew to false when saving
                company.IsNew = false;
                _companyRepository.Update(company);

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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var company = _companyRepository.GetById(id);

            if (!_companyService.IsDeactivable(company))
            {
                ModelState.AddModelError("Company", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _companyRepository.DeactivateAndCommit(company);
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var companys = new List<Company>();
            foreach (long id in selectedIds)
            {
                var company = _companyRepository.GetById(id);
                if (company != null)
                {
                    if (!_companyService.IsDeactivable(company))
                    {
                        ModelState.AddModelError("Company", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        companys.Add(company);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var company in companys)
                    _companyRepository.Deactivate(company);
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
        public JsonResult VendorList(string param)
        {
            var securityGroupIds = this._workContext.CurrentUser.SecurityGroups.Select(s => s.Id).ToList();
            var vendors = _companyRepository.GetAll()
                .Where(s => s.CompanyType.Name.Contains("Vendor") && s.Name.Contains(param))
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
                .ToList();
            if (vendors.Count > 0)
            {
                vendors.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(vendors);
        }

        #endregion

        #region Contacts

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Read,Purchasing.Contact.Read")]
        public ActionResult ContactList(long companyId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _contactRepository.GetAll().Where(c => c.CompanyId == companyId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var contacts = new PagedList<Contact>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = contacts.Select(x => x.ToModel()),
                Total = contacts.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Read,Purchasing.Contact.Create,Purchasing.Contact.Read,Purchasing.Contact.Update")]
        public ActionResult Contact(long id)
        {
            var contact = _contactRepository.GetById(id);
            var model = contact.ToModel();
            var html = this.ContactPanel(model);
            return Json(new { Id = contact.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create")]
        public ActionResult CreateContact(long companyId)
        {
            var contact = new Contact
            {
                IsNew = true
            };
            _contactRepository.Insert(contact);

            var company = _companyRepository.GetById(companyId);
            company.Contacts.Add(contact);

            this._dbContext.SaveChanges();

            var model = new ContactModel();
            model = contact.ToModel();
            var html = this.ContactPanel(model);

            return Json(new { Id = contact.Id, Html = html });
        }

        [NonAction]
        public string ContactPanel(ContactModel model)
        {
            var html = this.RenderPartialViewToString("_ContactDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult SaveContact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = _contactRepository.GetById(model.Id);
                //always set IsNew to false when saving
                contact.IsNew = false;
                contact = model.ToEntity(contact);

                _contactRepository.UpdateAndCommit(contact);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult CancelContact(long id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact.IsNew == true)
            {
                _contactRepository.DeleteAndCommit(contact);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Delete")]
        public ActionResult DeleteContact(long? parentId, long id)
        {
            var contact = _contactRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _contactRepository.DeactivateAndCommit(contact);
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Delete")]
        public ActionResult DeleteSelectedContacts(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var contact = _contactRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _contactRepository.Deactivate(contact);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Addresses

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Read,Purchasing.Company.Update")]
        public ActionResult AddressList(long companyId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var addresses = _companyRepository.GetById(companyId).Addresses;
            if (addresses.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = addresses.AsQueryable<Address>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = addresses.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Read,Purchasing.Company.Update")]
        public ActionResult Address(long id)
        {
            var address = _addressRepository.GetById(id);
            var model = address.ToModel();
            var html = this.AddressPanel(model);
            return Json(new { Id = address.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult CreateAddress(long companyId)
        {
            var address = new Address
            {
                IsNew = true
            };
            _addressRepository.Insert(address);

            var company = _companyRepository.GetById(companyId);
            company.Addresses.Add(address);

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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult DeleteAddress(long? parentId, long id)
        {
            var company = _companyRepository.GetById(parentId);
            var address = _addressRepository.GetById(id);
            //For many-many, need to remove from parent
            company.Addresses.Remove(address);

            _addressRepository.UpdateAndCommit(address);

            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Company.Create,Purchasing.Company.Update")]
        public ActionResult DeleteSelectedAddresses(long? parentId, long[] selectedIds)
        {
            var company = _companyRepository.GetById(parentId);
            foreach (long id in selectedIds)
            {
                var address = _addressRepository.GetById(id);
                //For many-many, need to remove from parent
                company.Addresses.Remove(address);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}
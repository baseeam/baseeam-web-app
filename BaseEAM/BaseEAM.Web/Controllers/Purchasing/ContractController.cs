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
using System;

namespace BaseEAM.Web.Controllers
{
    public class ContractController : BaseController
    {
        #region Fields

        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<ContractTerm> _contractTermRepository;
        private readonly IRepository<ContractPriceItem> _contractPriceItemRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<PreventiveMaintenance> _pmRepository;
        private readonly IContractService _contractService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ContractController(IRepository<Contract> contractRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IRepository<Contact> contactRepository,
            IRepository<ContractTerm> contractTermRepository,
            IRepository<ContractPriceItem> contractPriceItemRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<PreventiveMaintenance> pmRepository,
            IContractService contractService,
            IAutoNumberService autoNumberService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._contractRepository = contractRepository;
            this._assignmentRepository = assignmentRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._contactRepository = contactRepository;
            this._contractTermRepository = contractTermRepository;
            this._contractPriceItemRepository = contractPriceItemRepository;
            this._workOrderRepository = workOrderRepository;
            this._pmRepository = pmRepository;
            this._localizationService = localizationService;
            this._contractService = contractService;
            this._autoNumberService = autoNumberService;
            this._dateTimeHelper = dateTimeHelper;
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
            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Common.Number",
                DbColumn = "Contract.Number, Contract.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(numberFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = this._workContext.CurrentUser.DefaultSiteId,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(siteFilter);

            var priorityFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "Priority",
                ResourceKey = "Priority",
                DbColumn = "Contract.Priority",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Urgent,High,Medium,Low",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(priorityFilter);

            var statusFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Status",
                ResourceKey = "Common.Status",
                DbColumn = "Assignment.Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,WaitingForApproval,Approved,Rejected,Expired,Closed",
                CsvValueList = "Open,WaitingForApproval,Approved,Rejected,Expired,Closed",
                IsRequiredField = false
            };
            model.Filters.Add(statusFilter);

            return model;
        }

        #endregion

        #region Contracts

        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ContractSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ContractSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ContractSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ContractSearchModel] = model;

                PagedResult<Contract> data = _contractService.GetContracts(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var item in result)
                {
                    item.PriorityText = item.Priority.ToString();
                }
                var gridModel = new DataSourceResult
                {
                    Data = result,
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create")]
        public ActionResult Create()
        {
            var contract = new Contract
            {
                IsNew = true,
                Priority = (int?)AssignmentPriority.Medium,
                CreatedUserId = this._workContext.CurrentUser.Id
            };
            _contractRepository.InsertAndCommit(contract);

            //start workflow
            var workflowInstanceId = WorkflowServiceClient.StartWorkflow(contract.Id, EntityType.Contract, 0, this._workContext.CurrentUser.Id);
            return Json(new { Id = contract.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            var contract = _contractRepository.GetById(id);
            var assignment = contract.Assignment;
            var assignmentHistories = _assignmentHistoryRepository.GetAll()
                .Where(a => a.EntityId == contract.Id && a.EntityType == EntityType.Contract)
                .ToList();

            _contractRepository.Delete(contract);
            _assignmentRepository.Delete(assignment);
            foreach (var history in assignmentHistories)
                _assignmentHistoryRepository.Delete(history);

            this._dbContext.SaveChanges();

            //cancel wf
            WorkflowServiceClient.CancelWorkflow(
                contract.Id, EntityType.Contract, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                assignment.WorkflowVersion.Value, this._workContext.CurrentUser.Id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Read,Purchasing.Contract.Update")]
        public ActionResult Edit(long id)
        {
            var contract = _contractRepository.GetById(id);
            var model = contract.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult Edit(ContractModel model)
        {
            var contract = _contractRepository.GetById(model.Id);
            var assignment = contract.Assignment;
            if (ModelState.IsValid)
            {
                contract = model.ToEntity(contract);

                if (contract.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), contract);
                    contract.Number = number;
                }
                //always set IsNew to false when saving
                contract.IsNew = false;
                //copy to Assignment
                if (contract.Assignment != null)
                {
                    contract.Assignment.Number = contract.Number;
                    contract.Assignment.Description = contract.Description;
                    contract.Assignment.Priority = contract.Priority;
                }

                _contractRepository.Update(contract);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //trigger workflow action
                if (!string.IsNullOrEmpty(model.ActionName))
                {
                    WorkflowServiceClient.TriggerWorkflowAction(contract.Id, EntityType.Contract, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                        assignment.WorkflowVersion.Value, model.ActionName, model.Comment, this._workContext.CurrentUser.Id);
                    //Every time we query twice, because EF is caching entities so it won't get the latest value from DB
                    //We need to detach the specified entity and load it again
                    this._dbContext.Detach(contract.Assignment);
                    assignment = _assignmentRepository.GetById(contract.AssignmentId);
                }

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new
                {
                    number = contract.Number,
                    status = assignment.Name,
                    assignedUsers = assignment.Users.Select(u => u.Name),
                    availableActions = assignment.AvailableActions ?? ""
                });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var contract = _contractRepository.GetById(id);

            if (!_contractService.IsDeactivable(contract))
            {
                ModelState.AddModelError("Contract", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                var assignment = contract.Assignment;
                var assignmentHistories = _assignmentHistoryRepository.GetAll()
                    .Where(a => a.EntityId == contract.Id && a.EntityType == EntityType.Contract)
                    .ToList();

                _contractRepository.Deactivate(contract);
                _assignmentRepository.Deactivate(assignment);
                foreach (var history in assignmentHistories)
                    _assignmentHistoryRepository.Deactivate(history);

                this._dbContext.SaveChanges();

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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var contracts = new List<Contract>();
            foreach (long id in selectedIds)
            {
                var contract = _contractRepository.GetById(id);
                if (contract != null)
                {
                    if (!_contractService.IsDeactivable(contract))
                    {
                        ModelState.AddModelError("Contract", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        contracts.Add(contract);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var contract in contracts)
                {
                    var assignment = contract.Assignment;
                    var assignmentHistories = _assignmentHistoryRepository.GetAll()
                        .Where(a => a.EntityId == contract.Id && a.EntityType == EntityType.Contract)
                        .ToList();

                    _contractRepository.Deactivate(contract);
                    _assignmentRepository.Deactivate(assignment);
                    foreach (var history in assignmentHistories)
                        _assignmentHistoryRepository.Deactivate(history);
                }
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

        #region Contacts

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read,Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult ContactList(long contractId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _contactRepository.GetAll().Where(c => c.ContractId == contractId);
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read")]
        public ActionResult Contact(long id)
        {
            var contact = _contactRepository.GetById(id);
            var model = contact.ToModel();
            var html = this.ContactPanel(model);
            return Json(new { Id = contact.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create")]
        public ActionResult CreateContact(long contractId)
        {
            var contact = new Contact
            {
                IsNew = true
            };
            _contactRepository.Insert(contact);

            var contract = _contractRepository.GetById(contractId);
            contract.Contacts.Add(contact);

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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
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
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Delete")]
        public ActionResult DeleteContact(long? parentId, long id)
        {
            var contact = _contactRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _contactRepository.DeactivateAndCommit(contact);
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Delete")]
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

        #region Contract Terms

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Read,Purchasing.Contract.Update")]
        public ActionResult ContractTermList(long contractId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _contractTermRepository.GetAll().Where(c => c.ContractId == contractId);
            query = sort == null ? query.OrderBy(a => a.Sequence) : query.Sort(sort);
            var contractTerms = new PagedList<ContractTerm>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = contractTerms.Select(x => x.ToModel()),
                Total = contractTerms.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult SaveChanges(long contractId, [Bind(Prefix = "updated")]List<ContractTermModel> updatedItems,
           [Bind(Prefix = "created")]List<ContractTermModel> createdItems,
           [Bind(Prefix = "deleted")]List<ContractTermModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var contractTerm = model.ToEntity();
                            contractTerm.ContractId = contractId;
                            _contractTermRepository.Insert(contractTerm);
                        }
                    }

                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var contractTerm = _contractTermRepository.GetById(model.Id);
                            contractTerm = model.ToEntity(contractTerm);
                        }
                    }

                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var contractTerm = _contractTermRepository.GetById(model.Id);
                            _contractTermRepository.Deactivate(contractTerm);
                        }
                    }

                    _dbContext.SaveChanges();
                    SuccessNotification(_localizationService.GetResource("Record.Saved"));
                    return new NullJsonResult();
                }
                catch (System.Exception e)
                {
                    return Json(new { Errors = e.Message });
                }
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region ContractPriceItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Read,Purchasing.Contract.Update")]
        public ActionResult ContractPriceItemList(long contractId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _contractPriceItemRepository.GetAll().Where(c => c.ContractId == contractId);
            query = sort == null ? query.OrderBy(a => a.Item.Name) : query.Sort(sort);
            var contractPriceItems = new PagedList<ContractPriceItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = contractPriceItems.Select(x => x.ToModel()),
                Total = contractPriceItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Read,Purchasing.Contract.Update")]
        public ActionResult ContractPriceItem(long id)
        {
            var contractPriceItem = _contractPriceItemRepository.GetById(id);
            var model = contractPriceItem.ToModel();
            var html = this.ContractPriceItemPanel(model);
            return Json(new { Id = contractPriceItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult CreateContractPriceItem(long contractId)
        {
            //need to get contract here to assign to new contractPriceItem
            //so when mapping to Model, we will have StoreId as defined
            //in AutoMapper configuration
            var contract = _contractRepository.GetById(contractId);
            var contractPriceItem = new ContractPriceItem
            {
                IsNew = true,
                Contract = contract
            };
            _contractPriceItemRepository.Insert(contractPriceItem);

            this._dbContext.SaveChanges();

            var model = new ContractPriceItemModel();
            model = contractPriceItem.ToModel();
            var html = this.ContractPriceItemPanel(model);

            return Json(new { Id = contractPriceItem.Id, Html = html });
        }

        [NonAction]
        public string ContractPriceItemPanel(ContractPriceItemModel model)
        {
            var html = this.RenderPartialViewToString("_LineItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult SaveContractPriceItem(ContractPriceItemModel model)
        {
            if (ModelState.IsValid)
            {
                var contractPriceItem = _contractPriceItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                contractPriceItem.IsNew = false;
                contractPriceItem = model.ToEntity(contractPriceItem);
                _contractPriceItemRepository.UpdateAndCommit(contractPriceItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult CancelContractPriceItem(long id)
        {
            var contractPriceItem = _contractPriceItemRepository.GetById(id);
            if (contractPriceItem.IsNew == true)
            {
                _contractPriceItemRepository.DeleteAndCommit(contractPriceItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult DeleteContractPriceItem(long? parentId, long id)
        {
            var contractPriceItem = _contractPriceItemRepository.GetById(id);
            _contractPriceItemRepository.DeactivateAndCommit(contractPriceItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Create,Purchasing.Contract.Update")]
        public ActionResult DeleteSelectedContractPriceItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var contractPriceItem = _contractPriceItemRepository.GetById(id);
                _contractPriceItemRepository.Deactivate(contractPriceItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Work Order History

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read")]
        public ActionResult WorkOrderList(long? contractId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.ContractId == contractId);
            query = sort == null ? query.OrderBy(a => a.Number) : query.Sort(sort);
            var workOrders = new PagedList<WorkOrder>(query, command.Page - 1, command.PageSize);
            var result = workOrders.Select(x => x.ToModel()).ToList();
            foreach (var item in result)
            {
                item.PriorityText = item.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrders.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region PM History

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Purchasing.Contract.Read")]
        public ActionResult PMList(long? contractId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pmRepository.GetAll().Where(c => c.ContractId == contractId);
            query = sort == null ? query.OrderBy(a => a.Number) : query.Sort(sort);
            var pms = new PagedList<PreventiveMaintenance>(query, command.Page - 1, command.PageSize);
            var result = pms.Select(x => x.ToModel()).ToList();
            foreach (var item in result)
            {
                item.PriorityText = item.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = pms.TotalCount
            };

            return Json(gridModel);
        }

        #endregion
    }
}
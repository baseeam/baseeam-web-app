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
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class UserController : BaseController
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<SecurityGroup> _securityGroupRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserActivityService _userActivityService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly UserSettings _userSettings;
        private readonly IWorkContext _workContext;
        private readonly HttpContextBase _httpContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public UserController(IRepository<User> userRepository,
            IRepository<SecurityGroup> securityGroupRepository,
            IAuthenticationService authenticationService,
            IUserService userService,
            IUserRegistrationService userRegistrationService,
            IUserActivityService userActivityService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            UserSettings userSettings,
            IWorkContext workContext,
            HttpContextBase httpContext,
            IDbContext dbContext)
        {
            this._userRepository = userRepository;
            this._securityGroupRepository = securityGroupRepository;
            this._authenticationService = authenticationService;
            this._userService = userService;
            this._userRegistrationService = userRegistrationService;
            this._userActivityService = userActivityService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._userSettings = userSettings;
            this._workContext = workContext;
            this._httpContext = httpContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var userNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "UserName",
                ResourceKey = "User.Name",
                DbColumn = "User.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "User",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(userNameFilter);

            var securityGroupFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "SecurityGroupName",
                ResourceKey = "SecurityGroup.Name",
                DbColumn = "SecurityGroup_User.SecurityGroupId",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.DB,
                DbTable = "SecurityGroup",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(securityGroupFilter);

            return model;
        }

        #endregion

        #region User

        [BaseEamAuthorize(PermissionNames = "People.User.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.UserSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.UserSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.UserSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.UserSearchModel] = model;

                PagedResult<User> data = _userService.GetUsers(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "People.User.Create")]
        public ActionResult Create()
        {
            var user = new User { IsNew = true };
            _userRepository.InsertAndCommit(user);
            return Json(new { Id = user.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<User>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "People.User.Create,People.User.Read,People.User.Update")]
        public ActionResult Edit(long id)
        {
            var user = _userRepository.GetById(id);
            var model = user.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Create,People.User.Update")]
        public ActionResult Edit(UserModel model)
        {
            var user = _userRepository.GetById(model.Id);
            UpdateWebApiKeys(model, user);
            if (ModelState.IsValid)
            {
                user = model.ToEntity(user);
                //always set IsNew to false when saving
                user.IsNew = false;
                _userRepository.UpdateAndCommit(user);

                //password
                if (!string.IsNullOrWhiteSpace(model.LoginPassword))
                {
                    var changePassRequest = new ChangePasswordRequest(model.LoginName, false, _userSettings.DefaultPasswordFormat, model.LoginPassword);
                    var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                        foreach (var error in changePassResult.Errors)
                            ErrorNotification(error);
                }

                //commit all changes
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { publicKey = user.PublicKey, secretKey = user.SecretKey });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var user = _userRepository.GetById(id);

            if (!_userService.IsDeactivable(user))
            {
                ModelState.AddModelError("User", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _userRepository.DeactivateAndCommit(user);
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
        [BaseEamAuthorize(PermissionNames = "People.User.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var users = new List<User>();
            foreach (long id in selectedIds)
            {
                var user = _userRepository.GetById(id);
                if (user != null)
                {
                    if (!_userService.IsDeactivable(user))
                    {
                        ModelState.AddModelError("User", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        users.Add(user);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                    _userRepository.Deactivate(user);
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
        public ActionResult ChangePassword(long id, string loginName, string loginPassword)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var changePassRequest = new ChangePasswordRequest(loginName,
                    false, _userSettings.DefaultPasswordFormat, loginPassword);
                var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
                if (changePassResult.Success)
                    SuccessNotification(_localizationService.GetResource("User.PasswordChanged"));
                else
                    foreach (var error in changePassResult.Errors)
                        ErrorNotification(error);

                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        private void UpdateWebApiKeys(UserModel model, User user)
        {
            //new value != old value
            if (model.WebApiEnabled != user.WebApiEnabled)
            {
                if (model.WebApiEnabled == true)
                    _userService.CreateKeys(user);
                else
                    _userService.RemoveKeys(user);

                model.PublicKey = user.PublicKey;
                model.SecretKey = user.SecretKey;
            }
        }

        #endregion

        #region Login & Logout

        //[BaseEamHttpsRequirement(SslRequirement.No)]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _userRegistrationService.ValidateUser(model.LoginName, model.LoginPassword);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = _userService.GetUserByLoginName(model.LoginName);

                            //sign in new customer
                            _authenticationService.SignIn(user, false);

                            //activity log
                            _userActivityService.InsertActivityLog("Login", "From IP: " + Request.UserHostAddress);

                            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return Redirect("/");

                            return Redirect(returnUrl);
                        }
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("User.Login.WrongCredentials.UserNotExist"));
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("User.Login.WrongCredentials.Deleted"));
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("User.Login.WrongCredentials.NotActive"));
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("User.Login.WrongCredentials.NotRegistered"));
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("User.Login.WrongCredentials"));
                        break;
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            this._httpContext.Session.Clear();
            _authenticationService.SignOut();
            return RedirectToAction("Login", "User");
        }

        #endregion

        #region SecurityGroups

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Read")]
        public ActionResult SecurityGroupList(long userId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var securityGroups = _userRepository.GetById(userId).SecurityGroups;
            if (securityGroups.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = securityGroups.AsQueryable<SecurityGroup>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = securityGroups.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Create,People.User.Update")]
        public ActionResult AddSecurityGroups(long userId, long[] selectedIds)
        {
            var user = _userRepository.GetById(userId);
            foreach (var id in selectedIds)
            {
                var existed = user.SecurityGroups.Any(s => s.Id == id);
                if (!existed)
                {
                    var securityGroup = _securityGroupRepository.GetById(id);
                    user.SecurityGroups.Add(securityGroup);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Create,People.User.Update")]
        public ActionResult DeleteSecurityGroup(long? parentId, long id)
        {
            var user = _userRepository.GetById(parentId);
            var securityGroup = _securityGroupRepository.GetById(id);
            //For many-many, need to remove from parent
            user.SecurityGroups.Remove(securityGroup);
            _securityGroupRepository.UpdateAndCommit(securityGroup);

            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "People.User.Create,People.User.Update")]
        public ActionResult DeleteSelectedSecurityGroups(long? parentId, long[] selectedIds)
        {
            var user = _userRepository.GetById(parentId);

            foreach (long id in selectedIds)
            {
                var securityGroup = _securityGroupRepository.GetById(id);
                //For many-many, need to remove from parent
                user.SecurityGroups.Remove(securityGroup);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}
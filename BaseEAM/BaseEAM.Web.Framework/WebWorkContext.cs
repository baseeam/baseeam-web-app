/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Web;
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Core.Fakes;
using BaseEAM.Core.Data;
using System.Linq;
using BaseEAM.Core.Timing;

namespace BaseEAM.Web.Framework
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string UserCookieName = "baseeam.user";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILanguageService _languageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;

        private User _cachedUser;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;

        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IRepository<User> userRepository,
            IAuthenticationService authenticationService,
            ILanguageService languageService,
            LocalizationSettings localizationSettings,
            ICurrencyService currencyService,
            CurrencySettings currencySettings)
        {
            this._httpContext = httpContext;
            this._userRepository = userRepository;
            this._authenticationService = authenticationService;
            this._languageService = languageService;
            this._localizationSettings = localizationSettings;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        protected virtual void SetUserCookie(Guid userGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = userGuid.ToString();
                if (userGuid == Guid.Empty)
                {
                    cookie.Expires = Clock.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = Clock.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        protected virtual Language GetLanguageFromUrl()
        {
            return null;
        }

        protected virtual Language GetLanguageFromBrowserSettings()
        {
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //check whether request is made by a background task
                    //in this case return built-in user record for background task
                    user = _userRepository.GetAll().FirstOrDefault(u => u.Name == SystemUserNames.BackgroundTask && u.IsSystemAccount == true);
                }

                //registered user
                if (user == null || user.IsDeleted || !user.Active)
                {
                    user = _authenticationService.GetAuthenticatedUser();
                }

                //validation
                if (user != null && !user.IsDeleted && user.Active)
                {
                    SetUserCookie(user.UserGuid);
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                var language = new Language();
                var user = this.CurrentUser;

                //in case we don't have current user, like when login
                if (user == null)
                {
                    language = _languageService.GetLanguageById(1); //default to en_US
                }
                else
                {
                    language = _languageService.GetLanguageById(user.LanguageId.Value);
                }
                _cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                //reset cache
                _cachedLanguage = null;
            }
        }

        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        public virtual Currency WorkingCurrency
        {
            get
            {
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                //return primary store currency
                var primarySystemCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimarySystemCurrencyId);
                if (primarySystemCurrency != null)
                {
                    //cache
                    _cachedCurrency = primarySystemCurrency;
                    return primarySystemCurrency;
                }

                //cache
                _cachedCurrency = primarySystemCurrency;
                return _cachedCurrency;
            }
            set
            {
                //reset cache
                _cachedCurrency = null;
            }
        }

        #endregion
    }
}

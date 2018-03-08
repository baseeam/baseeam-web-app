/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class CurrencyController : BaseController
    {
        #region Fields

        private readonly IRepository<Currency> _currencyRepository;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly ISettingService _settingService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Constructors

        public CurrencyController(IRepository<Currency> currencyRepository,
            ICurrencyService currencyService,
            CurrencySettings currencySettings, ISettingService settingService,
            IDateTimeHelper dateTimeHelper, ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            this._currencyRepository = currencyRepository;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._settingService = settingService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        // GET: Currency
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(bool liveRates = false)
        {
            if (liveRates)
            {
                try
                {
                    var primaryExchangeCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryExchangeRateCurrencyId);
                    if (primaryExchangeCurrency == null)
                        throw new BaseEamException("Primary exchange rate currency is not set");

                    ViewBag.Rates = _currencyService.GetCurrencyLiveRates(primaryExchangeCurrency.CurrencyCode);
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc, false);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            var currenciesModel = _currencyService.GetAllCurrencies().Select(x => x.ToModel()).ToList();
            foreach (var currency in currenciesModel)
                currency.IsPrimaryExchangeRateCurrency = currency.Id == _currencySettings.PrimaryExchangeRateCurrencyId;
            foreach (var currency in currenciesModel)
                currency.IsPrimarySystemCurrency = currency.Id == _currencySettings.PrimarySystemCurrencyId;

            var gridModel = new DataSourceResult
            {
                Data = currenciesModel,
                Total = currenciesModel.Count
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ApplyRate(string currencyCode, decimal rate)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode);
            if (currency != null)
            {
                currency.Rate = rate;
                _currencyService.UpdateCurrency(currency);
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult MarkAsPrimaryExchangeRateCurrency(long id)
        {
            _currencySettings.PrimaryExchangeRateCurrencyId = id;
            _settingService.SaveSetting(_currencySettings);
            SuccessNotification(_localizationService.GetResource("Record.Saved"));

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult MarkAsPrimarySystemCurrency(long id)
        {
            _currencySettings.PrimarySystemCurrencyId = id;
            _settingService.SaveSetting(_currencySettings);
            SuccessNotification(_localizationService.GetResource("Record.Saved"));

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult Create()
        {
            var currency = new Currency { IsNew = true };
            _currencyService.InsertCurrency(currency);
            return Json(new { Id = currency.Id });
        }

        [HttpPost]
        public ActionResult Cancel(long? parentId, long id)
        {
            var currency = _currencyRepository.GetById(id);
            //hard delete
            _currencyService.DeleteCurrency(currency);
            return new NullJsonResult();
        }

        public ActionResult Edit(long id)
        {
            var currency = _currencyService.GetCurrencyById(id);
            var model = currency.ToModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CurrencyModel model)
        {
            var currency = _currencyRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                //in case of add new, change the IsNew flag to false before saving
                if (model.IsNew == true)
                {
                    model.IsNew = false;
                }
                currency = model.ToEntity(currency);
                _currencyService.UpdateCurrency(currency);

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
        public ActionResult Delete(long? parentId, long id)
        {
            var currency = _currencyRepository.GetById(id);

            if (!_currencyService.IsDeactivable(currency))
            {
                ModelState.AddModelError("Currency", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _currencyService.Deactivate(currency);
                _currencyService.UpdateCurrency(currency);
                //notification
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion
    }
}
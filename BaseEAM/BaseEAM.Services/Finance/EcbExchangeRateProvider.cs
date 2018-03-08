/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Timing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;

namespace BaseEAM.Services
{
    public class EcbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger _logger;

        public EcbExchangeRateProvider(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        public IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
        {
            if (exchangeRateCurrencyCode == null)
                throw new ArgumentNullException("exchangeRateCurrencyCode");

            //add euro with rate 1
            var ratesToEuro = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    CurrencyCode = "EUR",
                    Rate = 1,
                    ModifiedDateTime = DateTime.UtcNow
                }
            };

            //get exchange rates to euro from European Central Bank
            var request = (HttpWebRequest)WebRequest.Create("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
            try
            {
                using (var response = request.GetResponse())
                {
                    //load XML document
                    var document = new XmlDocument();
                    document.Load(response.GetResponseStream());

                    //add namespaces
                    var namespaces = new XmlNamespaceManager(document.NameTable);
                    namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                    namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                    //get daily rates
                    var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                    DateTime updateDate;
                    if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out updateDate))
                        updateDate = Clock.Now;

                    foreach (XmlNode currency in dailyRates.ChildNodes)
                    {
                        //get rate
                        decimal currencyRate;
                        if (!decimal.TryParse(currency.Attributes["rate"].Value, out currencyRate))
                            continue;

                        ratesToEuro.Add(new ExchangeRate()
                        {
                            CurrencyCode = currency.Attributes["currency"].Value,
                            Rate = currencyRate,
                            ModifiedDateTime = updateDate
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                _logger.Error("ECB exchange rate provider", ex);
            }

            //return result for the euro
            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
                return ratesToEuro;

            //use only currencies that are supported by ECB
            var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new BaseEamException("ECB exchange rate error");

            //return result for the selected (not euro) currency
            return ratesToEuro.Select(rate => new ExchangeRate
            {
                CurrencyCode = rate.CurrencyCode,
                Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
                ModifiedDateTime = rate.ModifiedDateTime
            }).ToList();
        }
    }
}

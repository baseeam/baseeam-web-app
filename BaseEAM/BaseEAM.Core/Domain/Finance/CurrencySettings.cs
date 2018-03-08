/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Configuration;
using System;

namespace BaseEAM.Core.Domain
{
    public class CurrencySettings : ISettings
    {
        public bool DisplayCurrencyLabel { get; set; }
        public long PrimarySystemCurrencyId { get; set; }
        public long PrimaryExchangeRateCurrencyId { get; set; }
        public bool AutoUpdateEnabled { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}

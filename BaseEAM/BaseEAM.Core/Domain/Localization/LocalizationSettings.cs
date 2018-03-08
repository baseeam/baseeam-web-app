/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Configuration;

namespace BaseEAM.Core.Domain
{
    public class LocalizationSettings : ISettings
    {
        /// <summary>
        /// Default language identifier
        /// </summary>
        public long DefaultLanguageId { get; set; }

        /// <summary>
        /// A value indicating whether to load all LocaleStringResource records on application startup
        /// </summary>
        public bool LoadAllLocaleRecordsOnStartup { get; set; }
    }
}

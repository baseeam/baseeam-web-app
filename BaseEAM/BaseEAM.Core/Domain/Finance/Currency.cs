/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Globalization;

namespace BaseEAM.Core.Domain
{
    public partial class Currency : BaseEntity
    {
        /// <summary>
        /// Gets or sets the currency code
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the rate
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets the display locale
        /// </summary>
        public string DisplayLocale { get; set; }

        /// <summary>
        /// Gets or sets the custom formatting
        /// </summary>
        public string CustomFormatting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a human-readable description associated with this
        /// currency. An example of such description would be, 'American Dollar', 
        /// 'Sterling Pound'
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a maximum five-character symbol to represent
        /// the currency. Examples of currency symbols are '$', '£', '€'.
        /// </summary>
        public string CurrencySymbol { get { return new RegionInfo(this.DisplayLocale).CurrencySymbol; } }
    }
}

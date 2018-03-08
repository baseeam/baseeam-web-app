/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Globalization;
using System.Text;

namespace BaseEAM.Services.Pdf
{
    public class PdfHeaderFooterOptions : IPdfOptions
    {

        public PdfHeaderFooterOptions()
        {
            Spacing = 5;
            FontSize = 10;
        }

        /// <summary>
        /// Spacing between footer and content in mm (default 0)
        /// </summary>
        public float? Spacing { get; set; }

        /// <summary>
        /// Display line below the header / above the footer
        /// </summary>
        public bool ShowLine { get; set; }

        /// <summary>
        /// Set font name (default Arial)
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Set font size (default 12)
        /// </summary>
        public float? FontSize { get; set; }

        /// <summary>
        /// Left aligned text
        /// </summary>
        public string TextLeft { get; set; }

        /// <summary>
        /// Centered text
        /// </summary>
        public string TextCenter { get; set; }

        /// <summary>
        /// Right aligned text
        /// </summary>
        public string TextRight { get; set; }

        public bool HasText
        {
            get { return !string.IsNullOrWhiteSpace(TextLeft) 
                    || !string.IsNullOrWhiteSpace(TextCenter) 
                    || !string.IsNullOrWhiteSpace(TextRight); }
        }

        public void Process(string flag, StringBuilder builder)
        {
            if (Spacing.HasValue)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, " --{0}-spacing {1}", flag, Spacing.Value);
            }

            if (ShowLine)
            {
                builder.AppendFormat(" --{0}-line", flag);
            }

            if (HasText)
            {
                if (!string.IsNullOrWhiteSpace(FontName))
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, " --{0}-font-name \"{1}\"", flag, FontName);
                }
                if (FontSize.HasValue)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, " --{0}-font-size {1}", flag, FontSize.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(TextLeft))
            {
                builder.AppendFormat(CultureInfo.CurrentCulture, " --{0}-left \"{1}\"", flag, TextLeft);
            }
            if (!string.IsNullOrWhiteSpace(TextCenter))
            {
                builder.AppendFormat(CultureInfo.CurrentCulture, " --{0}-center \"{1}\"", flag, TextCenter);
            }
            if (!string.IsNullOrWhiteSpace(TextRight))
            {
                builder.AppendFormat(CultureInfo.CurrentCulture, " --{0}-right \"{1}\"", flag, TextRight);
            }
        }

    }
}

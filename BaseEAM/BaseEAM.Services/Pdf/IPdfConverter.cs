/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Services.Pdf
{
    public interface IPdfConverter
    {
        /// <summary>
        /// Converts html content to PDF
        /// </summary>
        /// <param name="settings">The settings to be used for the conversion process</param>
        /// <returns>The PDF binary data</returns>
        byte[] Convert(PdfConvertSettings settings);
    }
}

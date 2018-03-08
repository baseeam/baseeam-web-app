/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Text;

namespace BaseEAM.Services.Pdf
{
    public interface IPdfOptions
    {
        /// <summary>
        /// Processes the options by converting them to native arguments
        /// </summary>
        /// <param name="flag">The section flag</param>
        /// <param name="sb">The builder</param>
        /// <remarks>Possible flags are: page | header | footer | cover | toc</remarks>
        void Process(string flag, StringBuilder builder);
    }
}

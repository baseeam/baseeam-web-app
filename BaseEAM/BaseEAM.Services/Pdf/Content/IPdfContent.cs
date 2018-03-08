/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Text;

namespace BaseEAM.Services.Pdf
{
    public interface IPdfContent
    {
        PdfContentKind Kind { get; }

        string Process(string flag);

        void WriteArguments(string flag, StringBuilder builder);

        void Teardown();
    }

    public enum PdfContentKind
    {
        Html,
        Url
    }
}

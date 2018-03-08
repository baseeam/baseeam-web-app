/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class ImportProfile : BaseEntity
    {
        public int? FileTypeId { get; set; }
        public string EntityType { get; set; }

        public DateTime? LastRunStartDateTime { get; set; }
        public DateTime? LastRunEndDateTime { get; set; }

        public string ImportFileName { get; set; }
        public string LogFileName { get; set; }
    }

    public enum ImportFileType
    {
        CSV = 0,
        XLSX
    }
}

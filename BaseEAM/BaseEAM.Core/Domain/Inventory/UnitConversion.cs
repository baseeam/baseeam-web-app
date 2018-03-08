/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class UnitConversion : BaseEntity
    {
        public long? FromUnitOfMeasureId { get; set; }
        public virtual UnitOfMeasure FromUnitOfMeasure { get; set; }

        public long? ToUnitOfMeasureId { get; set; }
        public virtual UnitOfMeasure ToUnitOfMeasure { get; set; }

        public decimal? ConversionFactor { get; set; }
    }
}

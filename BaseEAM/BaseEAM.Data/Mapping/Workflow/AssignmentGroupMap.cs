/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssignmentGroupMap : BaseEamEntityTypeConfiguration<AssignmentGroup>
    {
        public AssignmentGroupMap()
        {
            this.ToTable("AssignmentGroup");
            this.Property(s => s.Description).HasMaxLength(512);
        }
    }
}

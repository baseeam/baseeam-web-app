/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Services
{
    public interface IEntityAttributeService : IBaseService
    {
        void UpdateEntityAttributes(long? entityId, string entityType, string json);
    }
}

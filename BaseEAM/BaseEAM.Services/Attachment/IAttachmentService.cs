/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Services
{
    public interface IAttachmentService : IBaseService
    {
        void CopyAttachments(long fromEntityId, string fromEntityType, long toEntityId, string toEntityType);
    }
}

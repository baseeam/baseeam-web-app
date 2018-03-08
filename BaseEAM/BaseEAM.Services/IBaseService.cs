/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;

namespace BaseEAM.Services
{
    public partial interface IBaseService
    {
        bool IsDeactivable(BaseEntity entity);
        void Deactivate(BaseEntity entity);
        void Activate(BaseEntity entity);
    }
}

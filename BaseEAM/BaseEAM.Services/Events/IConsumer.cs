/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Services
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}

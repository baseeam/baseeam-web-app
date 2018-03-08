/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Timing
{
    public static class ClockProviders
    {
        public static UtcClockProvider Utc { get; } = new UtcClockProvider();
    }
}

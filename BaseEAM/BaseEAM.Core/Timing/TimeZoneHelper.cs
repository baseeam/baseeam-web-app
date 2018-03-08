/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Timing
{
    public static class TimeZoneHelper
    {
        public static DateTime? Convert(DateTime? date, string fromTimeZoneId, string toTimeZoneId)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZoneId);
            var destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(toTimeZoneId);
            return TimeZoneInfo.ConvertTime(date.Value, sourceTimeZone, destinationTimeZone);
        }

        public static DateTime? ConvertFromUtc(DateTime? date, string toTimeZoneId)
        {
            return Convert(date, "UTC", toTimeZoneId);
        }

        public static DateTime? ConvertToUtc(DateTime? date, string fromTimeZoneId)
        {
            return Convert(date, fromTimeZoneId, "UTC");
        }
    }
}

/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Result { get; set; }
        public int TotalCount { get; set; }

        public PagedResult(IEnumerable<T> result, int totalCount)
        {
            this.Result = result;
            this.TotalCount = totalCount;
        }
    }
}

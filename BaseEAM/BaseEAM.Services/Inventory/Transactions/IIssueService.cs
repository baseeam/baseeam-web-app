/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IIssueService : IBaseService
    {
        PagedResult<Issue> GetIssues(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        PagedResult<IssueItem> GetApprovedIssueItems(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        void UpdateIssueCost(IssueItem issueItem);

        List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Issue issue);

        void Approve(Issue issue);
    }
}

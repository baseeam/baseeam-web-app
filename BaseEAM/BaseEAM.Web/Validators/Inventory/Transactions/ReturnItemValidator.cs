/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class ReturnItemValidator : BaseEamValidator<ReturnItemModel>
    {
        private readonly IRepository<IssueItem> _issueItemRepository;
        public ReturnItemValidator(ILocalizationService localizationService,
            IRepository<ReturnItem> returnItemRepository,
            IRepository<IssueItem> issueItemRepository)
        {
            this._issueItemRepository = issueItemRepository;

            RuleFor(x => x.ReturnQuantity).NotEmpty().WithMessage(localizationService.GetResource("ReturnItem.Quantity.Required"));
        }
    }
}
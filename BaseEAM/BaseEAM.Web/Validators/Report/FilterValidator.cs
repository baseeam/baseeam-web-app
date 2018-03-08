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
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class FilterValidator : BaseEamValidator<FilterModel>
    {
        private readonly IRepository<Filter> _filterRepository;
        public FilterValidator(ILocalizationService localizationService,
            IRepository<Filter> filterRepository)
        {
            this._filterRepository = filterRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Filter.Name.Required"));
            RuleFor(x => x.CsvTextList).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.CSV &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.CsvTextList.Required"));
            RuleFor(x => x.CsvValueList).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.CSV &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.CsvValueList.Required"));

            RuleFor(x => x.DbTable).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.DB &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.DbTable.Required"));
            RuleFor(x => x.DbTextColumn).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.DB &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.DbTextColumn.Required"));
            RuleFor(x => x.DbValueColumn).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.DB &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.DbValueColumn.Required"));

            RuleFor(x => x.SqlQuery).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.SQL &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.SqlQuery.Required"));
            RuleFor(x => x.SqlTextField).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.SQL &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.SqlTextField.Required"));
            RuleFor(x => x.SqlValueField).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.SQL &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.SqlValueField.Required"));

            RuleFor(x => x.MvcController).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.MVC &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.MvcController.Required"));
            RuleFor(x => x.MvcAction).NotEmpty()
                .When(x => x.DataSource == FieldDataSource.MVC &&
                    (x.ControlType == FieldControlType.DropDownList
                        || x.ControlType == FieldControlType.MultiSelectList))
                .WithMessage(localizationService.GetResource("Filter.MvcAction.Required"));

            RuleFor(x => x.LookupType).NotEmpty()
                .When(x => x.ControlType == FieldControlType.Lookup).WithMessage(localizationService.GetResource("Filter.LookupType.Required"));
            RuleFor(x => x.LookupTextField).NotEmpty()
                .When(x => x.ControlType == FieldControlType.Lookup).WithMessage(localizationService.GetResource("Filter.LookupTextField.Required"));
            RuleFor(x => x.LookupValueField).NotEmpty()
                .When(x => x.ControlType == FieldControlType.Lookup).WithMessage(localizationService.GetResource("Filter.LookupValueField.Required"));
        }

        private bool NoDuplication(FilterModel model)
        {
            var filter = _filterRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return filter == null;
        }
    }
}
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
    public class CodeValidator : BaseEamValidator<CodeModel>
    {
        private readonly IRepository<Code> _codeRepository;
        public CodeValidator(ILocalizationService localizationService, IRepository<Code> codeRepository)
        {
            this._codeRepository = codeRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Code.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Code.Name.Unique"));
        }

        private bool NoDuplication(CodeModel model)
        {
            var code = _codeRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return code == null;
        }
    }
}
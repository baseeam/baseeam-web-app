/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using FluentValidation;

namespace BaseEAM.Web.Framework.Validators
{
    public abstract class BaseEamValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseEamValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}

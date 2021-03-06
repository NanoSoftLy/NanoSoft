﻿using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class PastDateAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            if (value is DateTime date && date < DateTime.Now)
                return ValidationResult.Success;

            if (value is string strDate && strDate.ToDateTime() < DateTime.Now)
                return ValidationResult.Success;

            return new ValidationResult(errorMessage);
        }

        protected override string DefaultErrorMessage => SharedMessages.PastDateRequired;
    }
}
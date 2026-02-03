using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserApi.Application.Extensions
{
    public static class ModelValidationExtension
    {
        public static void AddToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}

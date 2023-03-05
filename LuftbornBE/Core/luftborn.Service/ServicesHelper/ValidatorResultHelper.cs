using luftborn.Ground;
using luftborn.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace luftborn.Service.ServicesHelper
{
    public static class ValidatorResultHelper
    {
        public static Task<TValidatorResult> Success<TValidatorResult>() where TValidatorResult : ValidatorResult, new()
        {
            return Task.FromResult(new TValidatorResult());
        }

        public static Task<ValidatorResult> Error(string errorMessage, ValidationStatus? status = null)
        {
            return Task.FromResult(new ValidatorResult { IsValid = false, Message = errorMessage, Status = status });
        }
        public static Task<ValidatorResult> Error(string errorMessage, Action<Dictionary<string, object>> addErrorData)
        {
            var result = new ValidatorResult { IsValid = false, Message = errorMessage };
            addErrorData(result.AdditionalData);
            return Task.FromResult(result);
        }
        /// <summary>
        /// run multiple validations and return first invalid, or success
        /// </summary>
        /// <typeparam name="TValidatorResult">validation result class type</typeparam>
        /// <param name="validators">list of validations</param>
        /// <returns>first invalid, or success result</returns>
        public static async Task<TValidatorResult> ValidateAll<TValidatorResult>(List<Task<TValidatorResult>> validators) where TValidatorResult : ValidatorResult, new()
        {
            var result = await GetFirstInvalidResult(validators);
            return result ?? await Success<TValidatorResult>();
        }
        /// <summary>
        /// run multiple validations and return first invalid
        /// </summary>
        /// <typeparam name="TValidatorResult">validation result class type</typeparam>
        /// <param name="validators">list of validations</param>
        /// <returns>first invalid result, or null</returns>
        public static async Task<TValidatorResult> GetFirstInvalidResult<TValidatorResult>(List<Task<TValidatorResult>> validators) where TValidatorResult : ValidatorResult, new()
        {
            await Task.WhenAll(validators);
            return validators.Select(a => a.Result).FirstOrDefault(a => !a.IsValid);
        }
    }
}

using luftborn.Ground;
using System.Collections.Generic;

namespace luftborn.Service.Models
{
    public abstract class ServiceResultBase
    {
        public ServiceResultBase()
        {
            IsValid = true;
            Errors = new List<string>();
            AdditionalData = new Dictionary<string, object>();
        }
        //Return Single Error Message, With Not Valid Flag
        public ServiceResultBase(string errorMessage, ValidationStatus? status = null)
        {
            IsValid = false;
            Errors = new List<string>() { errorMessage };
            Status = status;
            AdditionalData = new Dictionary<string, object>();
        }

        public ServiceResultBase(ValidatorResult validatorResult)
        {
            if (validatorResult.IsValid)
            {
                IsValid = true;
                Errors = new List<string>();
            }
            else
            {
                IsValid = false;
                Errors = new List<string>() { validatorResult.Message };
            }
            AdditionalData = validatorResult.AdditionalData ?? new Dictionary<string, object>();
        }

        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }
        public ValidationStatus? Status { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
    }
    public class ServiceResultDetail<T> : ServiceResultBase where T : class
    {
        public ServiceResultDetail() : base() { }

        public ServiceResultDetail(string errorMessage, ValidationStatus? status = null) : base(errorMessage, status) { }

        public ServiceResultDetail(ValidatorResult validatorResult) : base(validatorResult) { }

        public T Model { get; set; }
        public long SubTotalCount { get; set; }
    }
    public class ServiceResultList<T> : ServiceResultBase where T : class
    {
        public ServiceResultList() : base() { }
        public ServiceResultList(ValidatorResult validatorResult) : base(validatorResult) { }

        public List<T> Model { get; set; }
        public long Count { get; set; }
    }
}

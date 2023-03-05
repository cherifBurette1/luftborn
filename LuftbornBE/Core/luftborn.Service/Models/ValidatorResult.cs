using luftborn.Ground;
using System.Collections.Generic;

namespace luftborn.Service.Models
{
    public class ValidatorResult
    {
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public ValidationStatus? Status { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        public ValidatorResult()
        {
            IsValid = true;
            Message = string.Empty;
            AdditionalData = new Dictionary<string, object>();
        }
    }
}

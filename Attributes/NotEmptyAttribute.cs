using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Attributes
{
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is Guid guid) {
                return guid != Guid.Empty;
            }

            return false;
        }
    }
}

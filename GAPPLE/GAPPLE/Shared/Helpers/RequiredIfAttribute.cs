using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Helpers
{
    public sealed class RequiredIfAttribute : ValidationAttribute, IValidatableObject
    {
        private readonly string propertyName;
        private readonly object isValue;
        private readonly bool inverse;
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the depending property.</param>
        /// <param name="isValue">Required value. If <see cref="propertyName"/> is <see cref="isValue"/> then the property is not required.</param>
        /// <param name="inverse">If set to true, the value is not required when <see cref="propertyName"/> is
        /// not <see cref="isValue"/>.</param>
        public RequiredIfAttribute(string propertyName, object isValue, string message = null, bool inverse = false)
        {
            this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.isValue = isValue;
            this.inverse = inverse;
            this.message = message;
        }

        /// <inheritdoc />
        public override string FormatErrorMessage(string name)
        {
            var inverseString = !inverse ? string.Empty : "no ";
            var errorMessage = string.IsNullOrEmpty(message) ? $"El campo '{name}' es requerido cuando '{propertyName}' es {inverseString}'{isValue}'" : message;
            return ErrorMessage ?? errorMessage;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = ValidationResult.Success;
            var isRequired = validationContext.IsRequired(propertyName, isValue, inverse);

            if (!isRequired)
                return ValidationResult.Success;

            bool invalid = false;

            if (value == null)
                invalid = true;
            else if (value is string str && string.IsNullOrWhiteSpace(str))
                invalid = true;
            else if (value is System.Collections.IEnumerable enumerable && !(value is string))
                invalid = !enumerable.Cast<object>().Any();

            if(invalid)
            {
                var memberNames = validationContext.MemberName != null
                    ? new[] { validationContext.MemberName }
                    : null;

                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return validationResult;
        }
    }
}

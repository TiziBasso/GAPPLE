using System.ComponentModel.DataAnnotations;

namespace GAPPLE.Shared.Helpers
{
    internal static class IfHelper
    {
        public static bool IsRequired(this ValidationContext validationContext, string propertyName, object requiredValue, bool invert)
        {
            ArgumentNullExceptionHelper.ThrowIfNull(validationContext, nameof(validationContext));

            var owningType = validationContext!.ObjectType;
            var property = owningType.GetProperty(propertyName);

            if (property == null)
            {
                throw new NotSupportedException($"Can't find {propertyName} on searched type: {owningType.Name}");
            }

            var requiredIfTypeActualValue = property.GetValue(validationContext.ObjectInstance);

            if (requiredIfTypeActualValue == null && requiredValue != null)
            {
                return false;
            }

            if (!invert)
            {
                return requiredIfTypeActualValue == null || requiredIfTypeActualValue.Equals(requiredValue);
            }

            return (requiredValue != null && requiredIfTypeActualValue == null) || (requiredIfTypeActualValue != null && !requiredIfTypeActualValue.Equals(requiredValue));
        }
    }
}

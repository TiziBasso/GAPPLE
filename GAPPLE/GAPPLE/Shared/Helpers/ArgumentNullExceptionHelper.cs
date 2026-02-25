using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GAPPLE.Shared.Helpers
{
    internal static class ArgumentNullExceptionHelper
    {
        private static readonly string ArgumentEmptyString = "The value cannot be an empty string.";

        internal static void ThrowIfNull([NotNull] object argument, [CallerArgumentExpression("argument")] string paramName = null)
            => ArgumentNullException.ThrowIfNull(argument, paramName);

        internal static void ThrowIfNullOrEmpty([NotNull] string argument, [CallerArgumentExpression("argument")] string paramName = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                ThrowNullOrEmptyException(argument, paramName);
            }
        }

        [DoesNotReturn]
        private static void ThrowNullOrEmptyException(string argument, string paramName)
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);
            throw new ArgumentException(ArgumentEmptyString, paramName);
        }
    }
}

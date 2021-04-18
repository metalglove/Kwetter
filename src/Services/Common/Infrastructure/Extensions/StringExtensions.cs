namespace Kwetter.Services.Common.Infrastructure.Extensions
{
    /// <summary>
    /// Represents the <see cref="StringExtensions"/> class.
    /// Used to change the casing of strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the first character of the string to the lower invariant.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>Returns the updated string.</returns>
        public static string ToCamelCase(this string @string)
        {
            return @string.Length > 1 ? char.ToLowerInvariant(@string[0]) + @string[1..] : @string;
        }

        /// <summary>
        /// Converts the first character of the string to the upper invariant.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>Returns the updated string.</returns>
        public static string ToPascalCase(this string @string)
        {
            return @string.Length > 1 ? char.ToUpperInvariant(@string[0]) + @string[1..] : @string;
        }
    }
}

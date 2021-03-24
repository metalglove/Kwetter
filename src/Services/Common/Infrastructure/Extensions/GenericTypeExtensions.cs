using System;
using System.Linq;

namespace Kwetter.Services.Common.Infrastructure.Extensions
{
    /// <summary>
    /// Represents the <see cref="GenericTypeExtensions"/> class.
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Gets the generic type name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns name of the generic type.</returns>
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                string genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        /// <summary>
        /// Gets the generic type name.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns>Returns name of the generic type.</returns>
        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }
}

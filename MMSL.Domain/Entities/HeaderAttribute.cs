using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace MMSL.Domain.Entities
{
    /// <summary>
    /// Used to indicate an Excel column header name corresponding to annotated member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    // ReSharper disable once InheritdocConsiderUsage
    public class HeaderAttribute : Attribute
    {

        /// <summary>
        /// Create an instance of <see cref="HeaderAttribute"/> with Excel column header string provided.
        /// </summary>
        /// <param name="header"></param>
        // ReSharper disable once InheritdocConsiderUsage
        public HeaderAttribute(string header)
        {
            Header = header;
        }

        /// <summary>
        /// Gets Excel column header.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Returns a read-only dictionary of <see cref="PropertyInfo"/> keyed with corresponding <see cref="HeaderAttribute.Header"/> for use as in-memory cache.
        /// </summary>
        /// <typeparam name="T">Type to cache properties of.</typeparam>
        /// <param name="equalityComparer">Cache key equality comparer.</param>
        /// <returns>Read-only dictionary of <see cref="PropertyInfo"/> keyed with <see cref="string"/>.</returns>
        public static ReadOnlyDictionary<string, PropertyInfo> GetPropertyCache<T>(IEqualityComparer<string> equalityComparer)
        {
            Type type = typeof(T);
            Dictionary<string, PropertyInfo> resultData = new Dictionary<string, PropertyInfo>(equalityComparer);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                HeaderAttribute headerAttribute = propertyInfo.GetCustomAttribute<HeaderAttribute>();
                if (headerAttribute != null)
                {
                    resultData.Add(headerAttribute.Header, propertyInfo);
                }
            }
            return new ReadOnlyDictionary<string, PropertyInfo>(resultData);
        }

    }
}

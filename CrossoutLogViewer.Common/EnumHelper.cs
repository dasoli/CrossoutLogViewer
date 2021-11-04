using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace CrossoutLogView.Common
{
    public static class EnumHelper<T> where T : struct, IConvertible
    {
        public static IList<T> GetValues(Enum value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            var enumValues = new List<T>();

            foreach (var fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            return enumValues;
        }

        public static T Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string LookupResource(Type resourceManagerProvider, string resourceKey)
        {
            if (resourceManagerProvider is null)
                throw new ArgumentNullException(nameof(resourceManagerProvider));
            if (string.IsNullOrEmpty(resourceKey))
                throw new ArgumentException("Value cannot be null or empty.", nameof(resourceKey));
            foreach (var staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static |
                BindingFlags.NonPublic | BindingFlags.Public))
                if (staticProperty.PropertyType == typeof(ResourceManager))
                {
                    var resourceManager = (ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
                }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes[0].ResourceType != null)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            if (descriptionAttributes == null) return string.Empty;
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
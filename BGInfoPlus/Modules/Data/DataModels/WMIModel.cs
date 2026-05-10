using System;
using System.Management;
using BGInfoPlus.Modules.Core;

namespace BGInfoPlus.Modules.Data.DataModels
{
    /// <summary>
    /// Provides methods to execute WMI queries and retrieve data from the Windows Management Instrumentation
    /// </summary>
    internal class WMIModel
    {
        /// <summary>
        /// Executes a WMI query and returns the result as an object
        /// </summary>
        /// <param name="query">The WMI query string (e.g., "SELECT * FROM Win32_OperatingSystem")</param>
        /// <returns>The query result as an object, or null if the query fails</returns>
        public static object? ExecuteWMIQuery(string query)
        {
            try
            {
                using ManagementObjectSearcher searcher = new(query);
                var collection = searcher.Get();

                // If no results, return null
                if (collection == null || collection.Count == 0)
                    return null;

                // If single result, return the first object
                if (collection.Count == 1)
                {
                    var mo = collection.Cast<ManagementObject>().First();
                    return mo;
                }

                // For multiple results, return the collection
                return collection;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error executing WMI query '{query}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Executes a WMI query and returns a specific property value from the first result
        /// </summary>
        /// <param name="query">The WMI query string</param>
        /// <param name="propertyName">The name of the property to retrieve</param>
        /// <returns>The property value as string, or null if not found</returns>
        public static string? GetWMIPropertyValue(string query, string propertyName)
        {
            try
            {
                using ManagementObjectSearcher searcher = new(query);
                var collection = searcher.Get();

                if (collection == null || collection.Count == 0)
                    return null;

                var mo = collection.Cast<ManagementObject>().First();
                var property = mo[propertyName];

                return property?.ToString();
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting WMI property '{propertyName}' from query '{query}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Executes a WMI query and returns a specific property value converted to the specified type
        /// </summary>
        /// <typeparam name="T">The type to convert the property value to</typeparam>
        /// <param name="query">The WMI query string</param>
        /// <param name="propertyName">The name of the property to retrieve</param>
        /// <returns>The converted property value, or default(T) if not found or conversion fails</returns>
        public static T? GetWMIPropertyValue<T>(string query, string propertyName)
        {
            try
            {
                var stringValue = GetWMIPropertyValue(query, propertyName);
                if (string.IsNullOrEmpty(stringValue))
                    return default;

                return (T)Convert.ChangeType(stringValue, typeof(T));
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error converting WMI property '{propertyName}' to type {typeof(T).Name}: {ex.Message}", StatusSeverityType.Error);
                return default;
            }
        }

        /// <summary>
        /// Executes a WMI query and returns a collection of property values from all results
        /// </summary>
        /// <param name="query">The WMI query string</param>
        /// <param name="propertyName">The name of the property to retrieve</param>
        /// <returns>A list of property values as strings, or null if the query fails</returns>
        public static List<string>? GetWMIPropertyValues(string query, string propertyName)
        {
            try
            {
                using ManagementObjectSearcher searcher = new(query);
                var collection = searcher.Get();

                if (collection == null || collection.Count == 0)
                    return null;

                List<string> values = [];
                foreach (ManagementObject mo in collection.Cast<ManagementObject>())
                {
                    var property = mo[propertyName];
                    values.Add(property?.ToString() ?? string.Empty);
                }

                return values;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting WMI property values '{propertyName}' from query '{query}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Checks if a WMI query returns any results
        /// </summary>
        /// <param name="query">The WMI query string</param>
        /// <returns>True if the query returns results, false otherwise</returns>
        public static bool WMIQueryExists(string query)
        {
            try
            {
                using ManagementObjectSearcher searcher = new(query);
                var collection = searcher.Get();
                return collection != null && collection.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}

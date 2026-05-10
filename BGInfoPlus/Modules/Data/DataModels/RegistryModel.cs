using System;
using BGInfoPlus.Modules.Core;
using Microsoft.Win32;

namespace BGInfoPlus.Modules.Data.DataModels
{
    /// <summary>
    /// Provides methods to read data from the Windows Registry
    /// </summary>
    internal class RegistryModel
    {
        /// <summary>
        /// Gets a registry value from the specified key path and value name
        /// </summary>
        /// <param name="keyPath">The registry key path (e.g., "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion")</param>
        /// <param name="valueName">The name of the value to retrieve (null for default value)</param>
        /// <returns>The registry value as string, or null if not found</returns>
        public static string? GetRegistryValue(string keyPath, string? valueName = null)
        {
            TraceLogger.Log($"Attempting to read registry value: KeyPath='{keyPath}', ValueName='{valueName}'");
            try
            {
                // Parse the registry key path
                var registryKey = ParseRegistryPath(keyPath);
                TraceLogger.Log(registryKey != null ? $"Registry key found: {keyPath}" : $"Registry key not found: {keyPath}");
                if (registryKey == null)
                    return null;

                // Get the value
                var value = registryKey.GetValue(valueName);
                TraceLogger.Log(value != null ? $"Registry value found: {valueName} = {value}" : $"Registry value not found: {valueName}");
                return value?.ToString();
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error reading registry value: {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets a registry value and converts it to the specified type
        /// </summary>
        /// <typeparam name="T">The type to convert the value to</typeparam>
        /// <param name="keyPath">The registry key path</param>
        /// <param name="valueName">The name of the value to retrieve</param>
        /// <returns>The converted value, or default(T) if not found or conversion fails</returns>
        //public static T? GetRegistryValue<T>(string keyPath, string? valueName = null)
        //{
        //    TraceLogger.Log($"Attempting to read and convert registry value: KeyPath='{keyPath}', ValueName='{valueName}', TargetType='{typeof(T).Name}'");
        //    try
        //    {
        //        var stringValue = GetRegistryValue(keyPath, valueName);
        //        if (string.IsNullOrEmpty(stringValue))
        //            return default;

        //        return (T)Convert.ChangeType(stringValue, typeof(T));
        //    }
        //    catch
        //    {
        //        TraceLogger.Log($"Error converting registry value to type {typeof(T).Name}", StatusSeverityType.Error);
        //        return default;
        //    }
        //}

        /// <summary>
        /// Parses a registry path and returns the corresponding RegistryKey
        /// </summary>
        /// <param name="keyPath">The registry key path</param>
        /// <returns>The RegistryKey or null if parsing fails</returns>
        private static RegistryKey? ParseRegistryPath(string keyPath)
        {
            TraceLogger.Log($"Parsing registry path: {keyPath}");
            if (string.IsNullOrEmpty(keyPath))
            {
                TraceLogger.Log("Registry key path is null or empty.", StatusSeverityType.Warning);
                return null;
            }

            try
            {
                TraceLogger.Log($"Determining registry root for path: {keyPath}");
                // Handle different registry root prefixes
                if (keyPath.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase))
                {
                    return Registry.ClassesRoot.OpenSubKey(keyPath[18..].TrimStart('\\'));
                }
                else if (keyPath.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
                {
                    return Registry.CurrentUser.OpenSubKey(keyPath[18..].TrimStart('\\'));
                }
                else if (keyPath.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase))
                {
                    return Registry.LocalMachine.OpenSubKey(keyPath[19..].TrimStart('\\'));
                }
                else if (keyPath.StartsWith("HKEY_USERS", StringComparison.OrdinalIgnoreCase))
                {
                    return Registry.Users.OpenSubKey(keyPath[11..].TrimStart('\\'));
                }
                else if (keyPath.StartsWith("HKEY_CURRENT_CONFIG", StringComparison.OrdinalIgnoreCase))
                {
                    return Registry.CurrentConfig.OpenSubKey(keyPath[20..].TrimStart('\\'));
                }
                else
                {
                    TraceLogger.Log($"No registry root prefix found in path: {keyPath}. Assuming it's a relative path under CurrentUser.");
                    // Assume it's a relative path under CurrentUser
                    return Registry.CurrentUser.OpenSubKey(keyPath);
                }
            }
            catch
            {
                TraceLogger.Log($"Error parsing registry path: {keyPath}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Checks if a registry key exists
        /// </summary>
        /// <param name="keyPath">The registry key path</param>
        /// <returns>True if the key exists, false otherwise</returns>
        public static bool RegistryKeyExists(string keyPath)
        {
            try
            {
                var registryKey = ParseRegistryPath(keyPath);
                TraceLogger.Log(registryKey != null ? $"Registry key exists: {keyPath}" : $"Registry key does not exist: {keyPath}");
                return registryKey != null;
            }
            catch
            {
                TraceLogger.Log($"Error checking registry key existence: {keyPath}", StatusSeverityType.Error);
                return false;
            }
        }

        /// <summary>
        /// Checks if a registry value exists
        /// </summary>
        /// <param name="keyPath">The registry key path</param>
        /// <param name="valueName">The name of the value to check</param>
        /// <returns>True if the value exists, false otherwise</returns>
        public static bool RegistryValueExists(string keyPath, string? valueName = null)
        {
            TraceLogger.Log($"Checking existence of registry value: KeyPath='{keyPath}', ValueName='{valueName}'");
            try
            {
                var registryKey = ParseRegistryPath(keyPath);
                TraceLogger.Log(registryKey != null ? $"Registry key found for value existence check: {keyPath}" : $"Registry key not found for value existence check: {keyPath}");
                if (registryKey == null)
                    return false;

                var value = registryKey.GetValue(valueName);
                TraceLogger.Log(value != null ? $"Registry value exists: {valueName}" : $"Registry value does not exist: {valueName}");
                return value != null;
            }
            catch
            {
                TraceLogger.Log($"Error checking registry value existence: {keyPath} - {valueName}", StatusSeverityType.Error);
                return false;
            }
        }
    }
}

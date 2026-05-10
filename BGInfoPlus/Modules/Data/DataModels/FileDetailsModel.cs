using System;
using System.IO;
using System.Text;
using BGInfoPlus.Modules.Core;

namespace BGInfoPlus.Modules.Data.DataModels
{
    /// <summary>
    /// Provides methods to get file details, contents, timestamps, etc.
    /// </summary>
    internal class FileDetailsModel
    {
        public enum FileDetailType
        {
            Size,
            CreationTime,
            LastAccessTime,
            LastWriteTime,
            Attributes,
            Contents
        }

        /// <summary>
        /// Gets specific file details based on the requested detail type
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <param name="detailType">The type of file detail to retrieve</param>
        /// <returns>The requested file detail as object, or null if not available</returns>
        public static object? GetFileDetails(string filePath, FileDetailType detailType)
        {
            try
            {
                // Validate file path
                if (string.IsNullOrEmpty(filePath))
                {
                    TraceLogger.Log("File path is null or empty.", StatusSeverityType.Warning);
                    return null;
                }

                // Check if file exists
                if (!File.Exists(filePath))
                {
                    TraceLogger.Log($"File not found: {filePath}", StatusSeverityType.Warning);
                    return null;
                }

                return detailType switch
                {
                    FileDetailType.Size => GetFileSize(filePath),
                    FileDetailType.CreationTime => GetFileCreationTime(filePath),
                    FileDetailType.LastAccessTime => GetFileLastAccessTime(filePath),
                    FileDetailType.LastWriteTime => GetFileLastWriteTime(filePath),
                    FileDetailType.Attributes => GetFileAttributes(filePath),
                    FileDetailType.Contents => GetFileContents(filePath),
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file details for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file size in bytes
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The file size as long, or null if not available</returns>
        private static long? GetFileSize(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);
                return fileInfo.Length;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file size for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file creation time
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The creation time as DateTime, or null if not available</returns>
        private static DateTime? GetFileCreationTime(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);
                return fileInfo.CreationTime;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file creation time for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file last access time
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The last access time as DateTime, or null if not available</returns>
        private static DateTime? GetFileLastAccessTime(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);
                return fileInfo.LastAccessTime;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file last access time for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file last write time
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The last write time as DateTime, or null if not available</returns>
        private static DateTime? GetFileLastWriteTime(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);
                return fileInfo.LastWriteTime;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file last write time for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file attributes
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The file attributes as string, or null if not available</returns>
        private static string? GetFileAttributes(string filePath)
        {
            try
            {
                FileInfo fileInfo = new(filePath);
                return fileInfo.Attributes.ToString();
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting file attributes for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets the file contents
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The file contents as string, or null if not available</returns>
        private static string? GetFileContents(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error reading file contents for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Gets basic file information as a FileInfo object
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>FileInfo object or null if not available</returns>
        public static FileInfo? GetFileInfo(string filePath)
        {
            try
            {
                return new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting FileInfo for '{filePath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public static bool FileExists(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch
            {
                return false;
            }
        }
    }
}

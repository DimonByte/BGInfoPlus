using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using BGInfoPlus.Modules.Core;

namespace BGInfoPlus.Modules.Data.DataModels
{
    /// <summary>
    /// Provides methods to execute scripts and retrieve their output
    /// </summary>
    internal class ScriptModel
    {
        /// <summary>
        /// Executes a script and returns its output as a string
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <param name="arguments">Optional arguments to pass to the script</param>
        /// <param name="timeoutMs">Optional timeout in milliseconds (default 30000ms)</param>
        /// <returns>The script output as string, or null if execution fails</returns>
        public static string? ExecuteScript(string scriptPath, string? arguments = null, int timeoutMs = 30000)
        {
            try
            {
                // Validate script path
                if (string.IsNullOrEmpty(scriptPath))
                {
                    TraceLogger.Log("Script path is null or empty.", StatusSeverityType.Warning);
                    return null;
                }

                // Check if script file exists
                if (!File.Exists(scriptPath))
                {
                    TraceLogger.Log($"Script file not found: {scriptPath}", StatusSeverityType.Warning);
                    return null;
                }

                // Determine the script interpreter based on file extension
                string? interpreter = GetInterpreterForScript(scriptPath);
                if (string.IsNullOrEmpty(interpreter))
                {
                    TraceLogger.Log($"No interpreter found for script: {scriptPath}", StatusSeverityType.Warning);
                    return null;
                }

                // Prepare process start info
                ProcessStartInfo startInfo = new()
                {
                    FileName = interpreter,
                    Arguments = $"\"{scriptPath}\" {arguments ?? string.Empty}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // Execute the script
                using Process? process = Process.Start(startInfo);
                if (process == null)
                {
                    TraceLogger.Log($"Failed to start script process: {scriptPath}", StatusSeverityType.Error);
                    return null;
                }

                // Set timeout
                if (!process.WaitForExit(timeoutMs))
                {
                    process.Kill();
                    TraceLogger.Log($"Script execution timed out: {scriptPath}", StatusSeverityType.Warning);
                    return null;
                }

                // Read output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Log any errors
                if (!string.IsNullOrEmpty(error))
                {
                    TraceLogger.Log($"Script error output: {error}", StatusSeverityType.Warning);
                }

                // Return output if successful
                if (process.ExitCode == 0)
                {
                    return output?.Trim();
                }
                else
                {
                    TraceLogger.Log($"Script exited with code {process.ExitCode}: {scriptPath}", StatusSeverityType.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error executing script '{scriptPath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Executes a script and returns its output as a string (overload for simple execution)
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <returns>The script output as string, or null if execution fails</returns>
        public static string? ExecuteScript(string scriptPath)
        {
            return ExecuteScript(scriptPath, null, 30000);
        }

        /// <summary>
        /// Executes a script and returns its output as a string (overload with timeout)
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>The script output as string, or null if execution fails</returns>
        public static string? ExecuteScript(string scriptPath, int timeoutMs)
        {
            return ExecuteScript(scriptPath, null, timeoutMs);
        }

        /// <summary>
        /// Executes a script and returns its output as a string (overload with arguments and timeout)
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <param name="arguments">Arguments to pass to the script</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>The script output as string, or null if execution fails</returns>
        public static string? ExecuteScriptWithArguments(string scriptPath, string? arguments, int timeoutMs = 30000)
        {
            return ExecuteScript(scriptPath, arguments, timeoutMs);
        }

        /// <summary>
        /// Determines the appropriate interpreter for a script file based on its extension
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <returns>The interpreter executable name, or null if not supported</returns>
        private static string? GetInterpreterForScript(string scriptPath)
        {
            try
            {
                string? extension = Path.GetExtension(scriptPath)?.ToLowerInvariant();

                return extension switch
                {
                    ".bat" or ".cmd" => "cmd.exe",
                    ".ps1" => "powershell.exe",
                    ".vbs" => "wscript.exe",
                    ".js" => "cscript.exe",
                    ".sh" => "bash.exe",// For Unix-like scripts on Windows (with WSL or Cygwin)
                    _ => "cmd.exe",// Try to execute directly or use cmd as fallback
                };
            }
            catch
            {
                return "cmd.exe"; // Fallback to cmd
            }
        }

        /// <summary>
        /// Checks if a script file exists and is executable
        /// </summary>
        /// <param name="scriptPath">The path to the script file</param>
        /// <returns>True if the script exists and is executable, false otherwise</returns>
        public static bool ScriptExists(string scriptPath)
        {
            try
            {
                if (string.IsNullOrEmpty(scriptPath))
                    return false;

                if (!File.Exists(scriptPath))
                    return false;

                // Check if it's a valid script file type
                string? extension = Path.GetExtension(scriptPath)?.ToLowerInvariant();
                return extension != null &&
                       (extension == ".bat" || extension == ".cmd" ||
                        extension == ".ps1" || extension == ".vbs" ||
                        extension == ".js" || extension == ".sh");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a PowerShell script with specific parameters
        /// </summary>
        /// <param name="scriptPath">The path to the PowerShell script file</param>
        /// <param name="arguments">Arguments to pass to the script</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>The script output as string, or null if execution fails</returns>
        public static string? ExecutePowerShellScript(string scriptPath, string? arguments = null, int timeoutMs = 30000)
        {
            try
            {
                // Validate script path
                if (string.IsNullOrEmpty(scriptPath) || !File.Exists(scriptPath))
                {
                    TraceLogger.Log($"PowerShell script not found: {scriptPath}", StatusSeverityType.Warning);
                    return null;
                }

                // Prepare PowerShell process start info
                ProcessStartInfo startInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" {arguments ?? string.Empty}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // Execute the PowerShell script
                using Process? process = Process.Start(startInfo);
                if (process == null)
                {
                    TraceLogger.Log($"Failed to start PowerShell script process: {scriptPath}", StatusSeverityType.Error);
                    return null;
                }

                // Set timeout
                if (!process.WaitForExit(timeoutMs))
                {
                    process.Kill();
                    TraceLogger.Log($"PowerShell script execution timed out: {scriptPath}", StatusSeverityType.Warning);
                    return null;
                }

                // Read output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Log any errors
                if (!string.IsNullOrEmpty(error))
                {
                    TraceLogger.Log($"PowerShell script error output: {error}", StatusSeverityType.Warning);
                }

                // Return output if successful
                if (process.ExitCode == 0)
                {
                    return output?.Trim();
                }
                else
                {
                    TraceLogger.Log($"PowerShell script exited with code {process.ExitCode}: {scriptPath}", StatusSeverityType.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error executing PowerShell script '{scriptPath}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }
    }
}

using BGInfoPlus.Modules.Core;
using BGInfoPlus.Modules.Data.DataModels;

namespace BGInfoPlus.Modules.Data
{
    //Gets the raw system data for the InfoFieldParser/Formatter to then work with.
    //This will work by interfacing with the data models (Registry, WMI, FileContents, Scripts) to get the raw data and then pass it to the InfoFieldParser/Formatter for processing.

    //Data flow is like this
    //InfoFieldParser will read the field definition (e.g. <LastBoot>) and determine what data it needs to get (e.g. WMI class Win32_OperatingSystem LastBootUpTime property.
    //Then will pass it to the SystemDataProvider to get the raw data. The systemdataprovider will interface with the appropriate data model to get the right data.
    //SystemDataprovider will then return the data if the data is valid, or return null if the data is not valid (e.g. registry key not found, WMI query fails, script returns error, etc.)
    //InfoFieldParser > SystemDataProvider > DataModels (RegistryModel, WMIModel, ScriptModel, FileContentModel)
    internal class SystemDataProvider
    {
        public enum DataSourceType
        {
            Registry,
            WMI,
            Script,
            FileDetails,
            EnviornmentVariable
        }
        public enum DataType
        {
            String,
            Integer,
            Float,
            DateTime,
            Boolean
        }
        public class DataRequest
        {
            public DataSourceType SourceType { get; set; }
            public required string Query { get; set; } // This will be the registry key path, WMI query, script path, file path, or env variable name depending on the SourceType
            public required string QueryValueName { get; set; } // For registry queries, this will be the value name. For WMI, this can be the property name to retrieve from the first result. For scripts, this can be ignored or used as needed by the script. For file details, this can be ignored.
            public DataType ExpectedDataType { get; set; }

            // For FileDetails, we need to specify which detail type we want
            public FileDetailsModel.FileDetailType FileDetailType { get; set; } = FileDetailsModel.FileDetailType.Size;
        }

        public static object? GetData(DataRequest request)
        {
            try
            {
                object? result = null;

                result = request.SourceType switch
                {
                    DataSourceType.Registry => RegistryModel.GetRegistryValue(request.Query, request.QueryValueName),
                    DataSourceType.WMI => WMIModel.ExecuteWMIQuery(request.Query),
                    DataSourceType.Script => ScriptModel.ExecuteScript(request.Query),
                    DataSourceType.FileDetails => FileDetailsModel.GetFileDetails(request.Query, request.FileDetailType),
                    DataSourceType.EnviornmentVariable => Environment.GetEnvironmentVariable(request.Query),
                    _ => throw new NotSupportedException($"Data source type {request.SourceType} is not supported."),
                };

                // Validate the result against ExpectedDataType
                if (result != null && !ValidateDataType(result, request.ExpectedDataType))
                {
                    TraceLogger.Log($"Data type validation failed. Expected {request.ExpectedDataType}, but got {result.GetType().Name}. Query: {request.Query}", StatusSeverityType.Warning);
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                TraceLogger.Log($"Error getting data for query '{request.Query}': {ex.Message}", StatusSeverityType.Error);
                return null;
            }
        }

        /// <summary>
        /// Validates that the returned object matches the expected data type
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="expectedType">The expected data type</param>
        /// <returns>True if validation passes, false otherwise</returns>
        private static bool ValidateDataType(object? value, DataType expectedType)
        {
            if (value == null)
                return true; // Null is acceptable for any type

            Type valueType = value.GetType();

            return expectedType switch
            {
                DataType.String => valueType == typeof(string) ||
                                           valueType == typeof(DateTime) ||
                                           valueType == typeof(bool) ||
                                           valueType == typeof(int) ||
                                           valueType == typeof(long) ||
                                           valueType == typeof(float) ||
                                           valueType == typeof(double),
                DataType.Integer => valueType == typeof(int) ||
                                           valueType == typeof(long) ||
                                           valueType == typeof(short) ||
                                           valueType == typeof(byte) ||
                                           valueType == typeof(sbyte) ||
                                           valueType == typeof(ushort) ||
                                           valueType == typeof(uint) ||
                                           valueType == typeof(ulong),
                DataType.Float => valueType == typeof(float) ||
                                           valueType == typeof(double) ||
                                           valueType == typeof(decimal),
                DataType.DateTime => valueType == typeof(DateTime) ||
                                           valueType == typeof(DateTimeOffset),
                DataType.Boolean => valueType == typeof(bool),
                _ => true,// Unknown type, assume valid
            };
        }
    }
}

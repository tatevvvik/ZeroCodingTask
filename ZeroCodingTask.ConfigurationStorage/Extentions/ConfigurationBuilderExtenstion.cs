using Microsoft.Extensions.Configuration;

namespace ZeroCodingTask.ConfigurationStorage.Extentions
{
    public static class ConfigurationBuilderExtenstion
    {
        public static IConfigurationBuilder AddFileStorageConfiguration(this IConfigurationBuilder builder, string filePath, bool reloadOnChange = false)
        {
            return builder.Add(new FileStorageConfigurationSource(filePath, reloadOnChange));
        }
    }
}

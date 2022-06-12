using Microsoft.Extensions.Configuration;

namespace ZeroCodingTask.ConfigurationStorage
{
    public class FileStorageConfigurationSource : IConfigurationSource
    {
        private readonly string _fileStoragePath;
        private readonly bool _reloadOnChange;

        public FileStorageConfigurationSource(string fileStoragePath, bool reloadOnChange = false)
        {
            _fileStoragePath = fileStoragePath ?? throw new ArgumentNullException(nameof(fileStoragePath));
            _reloadOnChange = reloadOnChange;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new FileStorageConfigurationProvider(_fileStoragePath, _reloadOnChange);
    }
}

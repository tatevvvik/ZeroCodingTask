using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ZeroCodingTask.ConfigurationStorage
{
    public class FileStorageConfigurationProvider : ConfigurationProvider
    {
        private readonly string _fileStoragePath;
        private readonly bool _reloadOnChange;


        public FileStorageConfigurationProvider(string fileStoragePath, bool reloadOnChange = false)
        {
            _fileStoragePath = fileStoragePath ?? throw new ArgumentNullException(nameof(fileStoragePath));
            _reloadOnChange = reloadOnChange;

            if (_reloadOnChange)
            {
                var fScM = new ConfigFileStorageWatcher();

                ChangeToken.OnChange(() => fScM.GetReloadToken(), () => ReLoad());
            }
        }

        public override void Load()
        {
            Data.Clear();

            ReadConfigsFromFileRecursive(_fileStoragePath);
        }

        private void ReLoad()
        {
            Console.WriteLine("Reloading data");
            Load();
        }

        private void ReadConfigsFromFileRecursive(string filePath, string key = null)
        {
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var lineValue = line.Trim();

                    if (FileStorageLineValidator.Validate(line))
                    {
                        var splitedValues = lineValue.Split(':', 2);

                        if (splitedValues[0] == "FILE")
                        {
                            var reSplit = splitedValues[1].Split(':', 2);

                            var nodeName = reSplit[0];
                            var nodeLink = reSplit[1].Replace("\"", null);

                            var combinedKey = key != null ? key + ":" + nodeName : nodeName;

                            ReadConfigsFromFileRecursive(nodeLink, combinedKey);
                        }
                        else
                        {
                            var keyName = splitedValues[0];
                            var keyValue = splitedValues[1].Replace("\"", null);

                            var combinedKey = key != null ? key + ":" + keyName : keyName;

                            Data.Add(combinedKey, keyValue);
                        }
                    }
                    else
                    {
                        throw new FormatException($"Unexpected line format. line {lineValue}");
                    }
                }
            }
        }
    }
}

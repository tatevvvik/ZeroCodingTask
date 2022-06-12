using Microsoft.Extensions.Primitives;
using ZeroCodingTask.ConfigurationStorage.Abstractions;

namespace ZeroCodingTask.ConfigurationStorage
{
    public class ConfigFileStorageWatcher : IFileStorageWatcher
    {
        private CancellationTokenSource _cts;
        private FileSystemWatcher _watcher;

        public ConfigFileStorageWatcher(string directory)
        {
            /// IMPORTANT NOTE !!!
            /// it will be better to have different directories rather than one called Configs
            /// and hence to have multiple watchers for different directories
            /// and when reloading reload only changed file keys determined by wathcer

            _watcher = new FileSystemWatcher(directory)
            {
                EnableRaisingEvents = true,
                Filter = "*.txt",
                NotifyFilter = NotifyFilters.Attributes |
                                    NotifyFilters.CreationTime |
                                    NotifyFilters.FileName |
                                    NotifyFilters.LastAccess |
                                    NotifyFilters.LastWrite |
                                    NotifyFilters.Size |
                                    NotifyFilters.Security,
            };

            _watcher.Changed += FileChangedHandler;
            _watcher.Deleted += FileChangedHandler;
            _watcher.Renamed += FileChangedHandler;
        }

        public void FileChangedHandler(object sender, FileSystemEventArgs e)
        {
            _cts?.Cancel();
        }

        public IChangeToken GetReloadToken()
        {
            _cts = new CancellationTokenSource();
            return new CancellationChangeToken(_cts.Token);
        }

        void IDisposable.Dispose()
        {
            _watcher.Dispose();
            _cts.Dispose();
        }
    }
}

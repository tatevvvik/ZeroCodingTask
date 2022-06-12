using Microsoft.Extensions.Primitives;

namespace ZeroCodingTask.ConfigurationStorage.Abstractions
{
    public interface IFileStorageWatcher : IDisposable
    {
        IChangeToken GetReloadToken();
    }
}

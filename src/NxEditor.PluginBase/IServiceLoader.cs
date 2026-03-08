using NxEditor.PluginBase.Models;
using NxEditor.PluginBase.Services;

namespace NxEditor.PluginBase;

public interface IServiceLoader
{
    Task<IFormatService> RequestService(IEditorFile handle);
    T? GetFirstService<T>(IEditorFile handle) where T : class, IFormatServiceProvider;
    IFormatServiceProvider? GetFirstService(IEditorFile handle);
    IServiceModule? GetService(string name);
    T? GetService<T>(string name) where T : class, IServiceModule;
    IEnumerable<IFormatServiceProvider> GetServices(IEditorFile handle);

    IServiceLoader Register(ITransformer service);
    IServiceLoader Register(string serviceId, IServiceModule service);
}

using NxEditor.PluginBase.Common;
using NxEditor.PluginBase.Models;
using NxEditor.PluginBase.Services;
using NxEditor.PluginBase.ViewModels;
using NxEditor.PluginBase.Views;

namespace NxEditor.PluginBase;

public class ServiceLoader : IServiceLoader
{
    public static ServiceLoader Shared { get; } = new();

    private readonly Dictionary<string, IServiceModule> _services = [];
    private readonly List<ITransformer> _processors = [];

    public async Task<IFormatService> RequestService(IEditorFile handle)
    {
        foreach (var processor in _processors) {
            if (processor.IsValid(handle)) {
                processor.TransformSource(handle);
                handle.Services.Add(processor);
            }
        }

        return await SelectServiceDialog(handle)
            ?? throw new NotSupportedException("The provided IFileHandle is not a supported data type");
    }

    private async Task<IFormatService?> SelectServiceDialog(IEditorFile handle)
    {
        KeyValuePair<string, IServiceModule>[] providers = _services
            .Where(x => x.Value.IsValid(handle))
            .ToArray();

        if (providers.Length <= 0) {
            return null;
        }

        if (providers.Length == 1) {
            return ((IFormatServiceProvider)providers[0].Value)
                .GetService(handle);
        }

        ItemSelectorViewModel context = new(providers.Select(x => x.Key));
        DialogBox dialog = new() {
            Title = "Editor Selection",
            IsSecondaryButtonVisible = false,
            Content = new ItemSelectorView {
                DataContext = context
            }
        };

        if (await dialog.ShowAsync() == DialogResult.Primary) {
            var provider = (IFormatServiceProvider)providers[context.Index].Value;
            return provider.GetService(handle);
        }
        else {
            throw new ApplicationException("The operation was cancelled");
        }
    }

    public T? GetFirstService<T>(IEditorFile handle) where T : class, IFormatServiceProvider
    {
        return GetServices(handle).FirstOrDefault(x => x is T) as T;
    }

    public IFormatServiceProvider? GetFirstService(IEditorFile handle)
    {
        return GetServices(handle).FirstOrDefault();
    }

    public IEnumerable<IFormatServiceProvider> GetServices(IEditorFile handle)
    {
        foreach (var processor in _processors) {
            if (processor.IsValid(handle)) {
                processor.TransformSource(handle);
                handle.Services.Add(processor);
            }
        }

        return _services
            .Where(x => x.Value.IsValid(handle))
            .Select(x => (x.Value as IFormatServiceProvider)!)
            .Where(x => x is not null);
    }

    public T? GetService<T>(string name) where T : class, IServiceModule => GetService(name) as T;
    public IServiceModule? GetService(string name)
    {
        return _services.TryGetValue(name, out var service) ? service : null;
    }

    public IServiceLoader Register(ITransformer service)
    {
        _processors.Add(service);
        return this;
    }

    public IServiceLoader Register(string serviceId, IServiceModule service)
    {
        _services.Add(serviceId, service);
        return this;
    }
}

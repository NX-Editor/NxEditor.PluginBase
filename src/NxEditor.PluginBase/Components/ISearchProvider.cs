using NxEditor.PluginBase.Services;

namespace NxEditor.PluginBase.Components;

public interface ISearchProvider : IFormatServiceProvider
{
    int Find(SearchContext context);
}

﻿using NxEditor.PluginBase.Models;
using NxEditor.PluginBase.Services;

namespace NxEditor.PluginBase;

public interface IServiceLoader
{
    public Task<IFormatService> RequestService(IEditorFile handle);
    public IServiceModule? RequestService(string name);
    public T? RequestService<T>(string name) where T : class, IServiceModule;

    public IServiceLoader Register(ITransformer service);
    public IServiceLoader Register(string serviceId, IServiceModule service);
}

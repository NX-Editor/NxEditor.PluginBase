﻿namespace NxEditor.PluginBase;

/// <summary>
/// Public extension interface
/// </summary>
public interface IServiceExtension
{
    public string Name { get; }
    public void RegisterExtension(IServiceLoader serviceManager);
}

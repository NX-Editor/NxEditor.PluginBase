﻿using NxEditor.PluginBase.Services;

namespace NxEditor.PluginBase.Components;

/// <summary>
/// 
/// </summary>
public interface IEditorInterface : IFormatService
{
    public virtual Task Save() => Save(Handle.Path);
    public Task Save(string? path);

    public virtual Task Undo() => Task.CompletedTask;
    public virtual Task Redo() => Task.CompletedTask;

    public virtual Task SelectAll() => Task.CompletedTask;
    public virtual Task Cut() => Task.CompletedTask;
    public virtual Task Copy() => Task.CompletedTask;
    public virtual Task Paste() => Task.CompletedTask;

    public virtual Task Find() => Task.CompletedTask;
    public virtual Task FindAndReplace() => Task.CompletedTask;
}
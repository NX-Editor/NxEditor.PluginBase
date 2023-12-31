﻿using Avalonia.Controls;
using NxEditor.PluginBase.Services;

namespace NxEditor.PluginBase.Components;

public interface IEditor : IEditorInterface, IFormatService
{
    public Control View { get; }
    public bool HasChanged { get; }
    public void Activate();
    public void Cleanup();
}

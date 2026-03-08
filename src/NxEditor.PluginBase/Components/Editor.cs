using Avalonia.Controls;
using Dock.Model.Mvvm.Controls;
using NxEditor.PluginBase.Common;
using NxEditor.PluginBase.Extensions;
using NxEditor.PluginBase.Models;
using NxEditor.PluginBase.Services;
using System.Diagnostics;

namespace NxEditor.PluginBase.Components;

public abstract class Editor<TView> : Document, IEditor, IEditorInterface, IFormatService where TView : Control, new()
{
    private static readonly Dictionary<string, IActionService> _actions = [];

    public Editor(IEditorFile handle)
    {
        Id = handle.Id;
        Title = handle.Name;

        Handle = handle;
        View = new() {
            DataContext = this
        };
    }

    public virtual bool HasChanged => false;
    Control IEditor.View => View;

    public TView View { get; }
    public Dictionary<string, IActionService> Actions => _actions;
    public IEditorFile Handle { get; protected set; }
    public abstract string[] ExportExtensions { get; }
    public virtual object? MenuModel { get; protected set; }
    public virtual object? FooterModel { get; protected set; }

    public abstract void Read();
    public abstract Span<byte> Write();

    public virtual void Save(string? path)
    {
        StatusModal.Set($"Saving {Title}", "fa-regular fa-floppy-disk");

        try {
            Span<byte> data = Write();
            foreach (var proc in Handle.Services) {
                proc.Transform(ref data, Handle);
            }

            if (path is not null) {
                EditorFile.WriteSafe(path, data);
            }
            else {
                Handle.Write(ref data);
            }

            StatusModal.Set($"Saved {Title} Successfully", "fa-regular fa-floppy-disk", false, 2);
        }
        catch (Exception ex) {
            StatusModal.Set($"Error Saving: {ex.Message}", "fa-regular fa-circle-xmark", isWorkingStatus: false, temporaryStatusTime: 3);
            Trace.WriteLine(ex);
        }

    }

    public virtual Task Undo() => Task.CompletedTask;
    public virtual Task Redo() => Task.CompletedTask;

    public virtual Task SelectAll() => Task.CompletedTask;
    public virtual Task Cut() => Task.CompletedTask;
    public virtual Task Copy() => Task.CompletedTask;
    public virtual Task Paste() => Task.CompletedTask;

    public virtual Task Find() => Task.CompletedTask;
    public virtual Task FindAndReplace() => Task.CompletedTask;

    public virtual void Activate() { }
    public virtual void Cleanup() { }

    public override void OnSelected()
    {
        // Update Menu
        if (EditorExtension.LastEditorMenu is not null) {
            Frontend.Locate<IMenuFactory>()
                .Remove(EditorExtension.LastEditorMenu);
        }

        EditorExtension.LastEditorMenu = MenuModel?.GetType();
        if (MenuModel is not null) {
            Frontend.Locate<IMenuFactory>()
                .Append(MenuModel);
        }

        // Update Footer
        if (EditorExtension.LastEditorFooter is not null) {
            Frontend.Locate<IFooterFactory>()
                .Remove(EditorExtension.LastEditorFooter);
        }

        EditorExtension.LastEditorFooter = FooterModel?.GetType();
        if (FooterModel is not null) {
            Frontend.Locate<IFooterFactory>()
                .Append(FooterModel);
        }

        base.OnSelected();
    }

    public override bool OnClose()
    {
        if (HasChanged) {
            DialogResult result = DialogBox.ShowAsync("Warning",
                "You have unsaved changes, are you sure you would like to exit?",
                primaryButtonContent: "Yes").WaitSynchronously();

            if (result != DialogResult.Primary) {
                return false;
            }
        }

        Cleanup();

        // Dereference View DataContext
        // View.DataContext = null;

        return base.OnClose();
    }
}

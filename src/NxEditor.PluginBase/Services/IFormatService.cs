using NxEditor.PluginBase.Models;

namespace NxEditor.PluginBase.Services;

/// <summary>
/// Handles an <see cref="IFileHandle"/> request as a file format specification
/// </summary>
public interface IFormatService
{
    /// <summary>
    /// Public dictionary of the available <see cref="IActionService"/> handles available for this <see cref="IFormatService"/>
    /// </summary>
    public Dictionary<string, IActionService> Actions { get; }

    /// <summary>
    /// The supported file extensions when writing the <see cref="IFormatProvider"/>
    /// </summary>
    public string[] ExportExtensions { get; }

    /// <summary>
    /// Stores the source <see cref="IFileHandle"/> as a save reference
    /// </summary>
    public IEditorFile Handle { get; }

    /// <summary>
    /// Reads the <see cref="Handle"/>
    /// </summary>
    public void Read();

    /// <summary>
    /// Writes the <see cref="IFormatService"/> and returns the data as <see cref="Span{T}"/>.
    /// </summary>
    /// <returns></returns>
    public Span<byte> Write();
}

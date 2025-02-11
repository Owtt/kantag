#if TOOLS
using System;
using Godot;

public partial class ResizeFormatProperty : EditorProperty
{
    public Action<long> onItemSelected;

    private OptionButton optionButton = new();

    public ResizeFormatProperty(ResizeFormat format, ResizingContainer container)
    {
        optionButton.AddItem("Vertical", 0);
        optionButton.AddItem("Horizontal", 1);

        AddChild(optionButton);
        AddFocusable(optionButton);

        optionButton.Select((int)format);

        onItemSelected += container.SetResizeFormat;

        optionButton.ItemSelected += onItemSelected.Invoke;
    }
}

public enum ResizeFormat
{
    Vertical = 0,
    Horizontal = 1,
    Grid = 2,
}
#endif

#if TOOLS
using System;
using Godot;

public partial class ResizeFormatProperty : EditorProperty
{
    private OptionButton optionButton = new();
    private int currentValue;

    private ResizingContainer resizeContainer;

    private bool _updating = false;

    public ResizeFormatProperty()
        : this(ResizeFormat.Vertical, null) { }

    public ResizeFormatProperty(ResizeFormat format, ResizingContainer container)
    {
        resizeContainer = container;

        optionButton.AddItem("Vertical", 0);
        optionButton.AddItem("Horizontal", 1);

        AddChild(optionButton);
        AddFocusable(optionButton);

        optionButton.Select((int)format);

        optionButton.ItemSelected += onItemSelected;

        optionButton.ItemSelected += container.SetResizeFormat;
    }

    public override void _ExitTree()
    {
        optionButton.ItemSelected -= onItemSelected;
        optionButton.ItemSelected -= resizeContainer.SetResizeFormat;
    }

    private void onItemSelected(long value)
    {
        if (_updating)
            return;

        currentValue = (int)value;
        EmitChanged(GetEditedProperty(), currentValue);
    }

    public override void _UpdateProperty()
    {
        if (optionButton == null)
            return;
        var newValue = (int)GetEditedObject().Get(GetEditedProperty());
        if (newValue == currentValue)
            return;

        _updating = true;
        currentValue = newValue;
        _updating = false;
    }
}

public enum ResizeFormat
{
    Vertical = 0,
    Horizontal = 1,
    Grid = 2,
}
#endif

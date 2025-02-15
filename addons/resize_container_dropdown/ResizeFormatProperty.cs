#if TOOLS
using System;
using Godot;

public partial class ResizeFormatProperty : EditorProperty
{
    private OptionButton optionButton = new();
    private int currentValue;

    private bool _updating = false;

    public ResizeFormatProperty(ResizeFormat format, ResizingContainer container)
    {
        optionButton.AddItem("Vertical", 0);
        optionButton.AddItem("Horizontal", 1);

        AddChild(optionButton);
        AddFocusable(optionButton);

        optionButton.Select((int)format);

        optionButton.ItemSelected += onItemSelected;

        optionButton.ItemSelected += container.SetResizeFormat;
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

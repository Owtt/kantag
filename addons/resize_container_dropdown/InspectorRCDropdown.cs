#if TOOLS
using Godot;

public partial class InspectorRCDropdown : EditorInspectorPlugin
{
    private ResizingContainer container;
    private ResizeFormatProperty property;

    public override bool _CanHandle(GodotObject @object)
    {
        if (@object is ResizingContainer rc)
        {
            container = rc;

            property = new ResizeFormatProperty(rc.Format, rc);

            return true;
        }

        return false;
    }

    public override bool _ParseProperty(
        GodotObject @object,
        Variant.Type type,
        string name,
        PropertyHint hintType,
        string hintString,
        PropertyUsageFlags usageFlags,
        bool wide
    )
    {
        if (name == "resizeFormat")
        {
            AddPropertyEditor(name, property);
            return true;
        }

        return false;
    }

    // public override void _ParseCategory(GodotObject @object, string category)
    // {
    //     if (category == "ResizingContainer")
    //     {
    //         // Label label = new();
    //         // label.Text = " Resize Format";
    //         // AddCustomControl(label);
    //         AddPropertyEditor("resizeFormat", property);
    //     }
    // }
}
#endif

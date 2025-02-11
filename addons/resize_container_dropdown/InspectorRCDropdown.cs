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

    public override void _ParseCategory(GodotObject @object, string category)
    {
        if (category == "ResizingContainer")
        {
            // Label label = new();
            // label.Text = " Resize Format";
            // AddCustomControl(label);
            AddPropertyEditor("ResizeFormat", property);
        }
    }
}
#endif

#if TOOLS
using Godot;

[Tool]
public partial class ResizeContainerDropdown : EditorPlugin
{
    InspectorRCDropdown dropdown;

    public override void _EnterTree()
    {
        dropdown = new();
        AddInspectorPlugin(dropdown);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(dropdown);
    }
}
#endif

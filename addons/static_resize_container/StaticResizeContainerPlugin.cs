#if TOOLS
using Godot;

[Tool]
public partial class StaticResizeContainerPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        var script = GD.Load<Script>("res://Systems/UI/StaticResizeContainer.cs");
        var texture = GD.Load<Texture2D>("res://Textures/White Square.png");
        AddCustomType("StaticResizeContainer", "Container", script, texture);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveCustomType("StaticResizeCotainer");
    }
}
#endif

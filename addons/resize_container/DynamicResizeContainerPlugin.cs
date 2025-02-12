#if TOOLS
using Godot;
using System;

[Tool]
public partial class DynamicResizeContainerPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        var script = GD.Load<Script>("res://Systems/UI/DynamicResizeContainer.cs");
        var texture = GD.Load<Texture2D>("res://Textures/White Square.png");
        AddCustomType("DynamicResizeContainer", "Container", script, texture);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveCustomType("DynamicResizeContainer");
    }
}
#endif

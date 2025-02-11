#if TOOLS
using Godot;
using System;

[Tool]
public partial class ResizeContainerPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        var script = GD.Load<Script>("res://Systems/UI/ResizingContainer.cs");
        var texture = GD.Load<Texture2D>("res://Textures/White Square.png");
        AddCustomType("ResizingContainer", "Container", script, texture);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveCustomType("ResizingContainer");
    }
}
#endif

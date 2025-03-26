using Godot;

public abstract partial class UIView : Control
{
    [Export]
    protected UserCardData cardData;

    [Export]
    protected Label label;

    [Export]
    protected DynamicResizeContainer childContainer;

    [Export]
    protected PackedScene prefab;

    public abstract void SetView(string viewName);
}

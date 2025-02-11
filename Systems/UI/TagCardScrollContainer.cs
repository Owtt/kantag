using Godot;

public partial class TagCardScrollContainer : VBoxContainer
{
    private ScrollContainer scrollContainer;
    private BoxContainer boxContainer;
    private PanelContainer panelContainer;

    private Vector2 panelContainerSize;
    private Vector2 startingSize;

    [Export]
    private UIData data;

    public override void _Ready()
    {
        scrollContainer = FindChild("TagScrollContainer") as ScrollContainer;
        boxContainer = FindChild("CardContainer") as BoxContainer;
        panelContainer = GetParent() as PanelContainer;

        startingSize = Size;
        panelContainerSize = panelContainer.Size;

        data.onSessionScrollSizeChanged += UpdateScrollContainer;
    }

    private void UpdateScrollContainer(Vector2 maxSize)
    {
        // UpdateScrollContainer

        Size = new Vector2(startingSize.X, startingSize.Y + boxContainer.Size.Y);

        panelContainer.Size = new Vector2(panelContainerSize.X, 20 + Size.Y);

        scrollContainer.SetSize(boxContainer.Size);
    }
}

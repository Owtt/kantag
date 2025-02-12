using System;
using Godot;

[Tool]
public partial class DynamicResizeContainer : ResizingContainer
{
    private Control childContainer;

    [Export]
    private Control scrollArea;

    // enable when children need to scroll as well
    [Export]
    private bool scrollOverChildren;

    [Export]
    protected float scrollSpeed = 10f;

    private Vector2 minScrollBounds;

    private bool scrollable = false;

    [Export]
    private bool debugSpawn = false;

    [Export]
    private int debugButton;

    [Export]
    private PackedScene debugPrefab;

    public override void _Ready()
    {
        childContainer = new Control();

        childContainer.ChildEnteredTree += ChildAdded;
        childContainer.ChildExitingTree += ChildRemoved;

        base.AddChild(childContainer);

        if (!scrollOverChildren)
        {
            if (scrollArea == null)
                scrollArea = GetParent() as Control;

            scrollArea.MouseEntered += OnMouseEnter;
            scrollArea.MouseExited += OnMouseExit;
        }

        base._Ready();
    }

    public override void _Input(InputEvent @event)
    {
        if (debugSpawn)
        {
            if (@event.IsActionPressed("DebugS" + debugButton))
            {
                TestDespawn();
            }
            else if (@event.IsActionPressed("Debug" + debugButton))
            {
                TestSpawn();
            }
        }

        if (@event.IsActionPressed("ScrollUp"))
        {
            CheckScrollable(true);
        }

        if (@event.IsActionPressed("ScrollDown"))
        {
            CheckScrollable(false);
        }

        if (!scrollOverChildren)
            return;

        if (@event is InputEventMouseMotion mouse)
        {
            Vector2 mPos = mouse.Position;
            Vector2 pPos = scrollArea.Position;
            Vector2 pSize = scrollArea.Size;

            bool inX = mPos.X > pPos.X && mPos.X < pPos.X + pSize.X;
            bool inY = mPos.Y > pPos.Y && mPos.Y < pPos.Y + pSize.Y;

            scrollable = inX && inY;
        }
    }

    private void ChildAdded(Node node)
    {
        UpdateSize();
    }

    private void ChildRemoved(Node node)
    {
        GD.Print(node.Name + " removed");
        Control c = node as Control;
        children.Remove(c);

        UpdateSize();
    }

    private void OnMouseEnter()
    {
        scrollable = true;
    }

    private void OnMouseExit()
    {
        scrollable = false;
    }

    private void AddChild(Node node)
    {
        // nodes added to this objects will be processed then added to a child object
        if (node is Control c)
        {
            childContainer.AddChild(node);

            children.Add(c);

            c.Resized += UpdateSize;
        }
        else
        {
            GD.PrintErr("Error: ResizingContainer is unable to accept non-control nodes.");
        }
    }

    private new void RemoveChild(Node node)
    {
        // when removing an object, attempt to remove from said child node
        childContainer.RemoveChild(node);
    }

    protected override void ResizeVerticalList()
    {
        float y = 0f;
        float largestX = 0f;

        // y += padTop;

        foreach (Control c in children)
        {
            if (c.Size.X > largestX)
                largestX = c.Size.X;

            c.Position = new Vector2(0, y);
            y += c.Size.Y;
            y += padContentY;
        }

        y -= padContentY;
        // y += padBot;

        SetSize(largestX, y);
        childContainer.Size = new Vector2(largestX, y);

        minScrollBounds.Y = Size.Y - childContainer.Size.Y;
    }

    protected override void ResizeHorizontalList()
    {
        float x = 0f;
        float largestY = 0f;

        x += padLeft;

        foreach (Control c in children)
        {
            if (c.Size.Y > largestY)
                largestY = c.Size.Y;

            c.Position = new Vector2(x, 0);
            x += c.Size.X;
            x += padContentX;
        }

        x -= padContentX;
        // x += padRight;

        SetSize(x, largestY);
    }

    private void ScrollContent(bool up)
    {
        float scrollMultiplier = up ? 1 : -1;
        float scroll = scrollSpeed * scrollMultiplier;

        Vector2 scrollPos;

        switch (Format)
        {
            case ResizeFormat.Horizontal:
                scrollPos = childContainer.Position;
                scrollPos.Y += scroll;
                scrollPos.Y = Math.Clamp(scrollPos.Y, minScrollBounds.Y, 0);

                childContainer.Position = scrollPos;
                break;
            case ResizeFormat.Vertical:
                scrollPos = childContainer.Position;
                scrollPos.Y += scroll;
                scrollPos.Y = Math.Clamp(scrollPos.Y, minScrollBounds.Y, 0);

                childContainer.Position = scrollPos;
                break;
            case ResizeFormat.Grid:
                break;
            default:
                break;
        }
    }

    private void CheckScrollable(bool up)
    {
        if (!scrollable)
            return;

        // scroll content
        if (scrollOverChildren)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();

            float xMin = GlobalPosition.X;
            float xMax = xMin + Size.X;
            float yMin = GlobalPosition.Y;
            float yMax = yMin + Size.Y;

            bool inX = mousePos.X >= xMin && mousePos.X <= xMax;
            bool inY = mousePos.Y >= yMin && mousePos.Y <= yMax;

            if (inX && inY)
            {
                scrollable = true;
            }
        }

        ScrollContent(up);
    }

    protected override void UpdateFormat(ResizeFormat nextFormat)
    {
        switch (Format)
        {
            case ResizeFormat.Horizontal:
                foreach (Control c in children)
                {
                    c.Resized -= ResizeHorizontalList;
                }
                break;
            case ResizeFormat.Vertical:
                foreach (Control c in children)
                {
                    c.Resized -= ResizeVerticalList;
                }
                break;
            case ResizeFormat.Grid:
                break;
            default:
                break;
        }

        switch (nextFormat)
        {
            case ResizeFormat.Horizontal:
                foreach (Control c in children)
                {
                    c.Resized += ResizeHorizontalList;
                }
                ResizeHorizontalList();
                break;
            case ResizeFormat.Vertical:
                foreach (Control c in children)
                {
                    c.Resized += ResizeVerticalList;
                }
                ResizeVerticalList();
                break;
            case ResizeFormat.Grid:
                break;
            default:
                break;
        }
    }

    private void TestSpawn()
    {
        Node n = debugPrefab.Instantiate<Node>();

        AddChild(n);
    }

    private void TestDespawn()
    {
        if (children.Count < 1)
            return;

        Control c = children[children.Count - 1];

        c.QueueFree();
    }
}

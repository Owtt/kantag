using System;
using System.Collections.Generic;
using Godot;

[Tool]
public partial class ResizingContainer : Container
{
    public Action onResized;

    public ResizeFormat Format => resizeFormat;

    public float PadTop => padTop;

    public float PadBot => padBot;

    public float PadRight => padRight;

    public float PadLeft => padLeft;

    private ResizeFormat resizeFormat;

    [Export]
    private bool staticChildren = true;

    [Export]
    private float padTop;

    [Export]
    private float padBot;

    [Export]
    private float padRight;

    [Export]
    private float padLeft;

    [Export]
    private float padContentY;

    [Export]
    private float padContentX;

    [Export]
    private bool debugSpawn = false;

    [Export]
    private int debugButton;

    [Export]
    private PackedScene debugPrefab;

    [Export]
    private bool useBounds;

    [Export]
    private bool updateParent = true;

    [Export]
    private Vector2 minBounds;

    [Export]
    private Vector2 maxBounds;

    [Export]
    private Control scrollArea;

    // enable when children need to scroll as well
    [Export]
    private bool scrollOverChildren;

    [Export]
    private float scrollSpeed;

    private Vector2 minScrollBounds;

    private bool scrollable = false;

    private int verticalMaxIndex;
    private int horizontalMaxIndex;

    private List<Control> children = new List<Control>();

    private Control parentContainer;
    private Control childContainer;

    public void CheckParentContainer()
    {
        if (parentContainer == null)
            parentContainer = GetParent() as MarginContainer;
    }

    // private Control firstObj
    // {
    //     get { return firstObj; }
    //     set
    //     {
    //         firstObj = value;
    //         EnableObjectsInView();
    //     }
    // }
    //
    // private Control GetFirstObj()
    // {
    //     if (firstObj == null)
    //     {
    //         firstObj = GetTopControl();
    //     }
    //
    //     return firstObj;
    // }
    //
    // private int topObjIdx;

    public override void _Ready()
    {
        parentContainer = GetParent() as Control;
        childContainer = new Control();

        childContainer.ChildEnteredTree += ChildAdded;
        childContainer.ChildExitingTree += ChildRemoved;

        if (!scrollOverChildren)
        {
            scrollArea.MouseEntered += OnMouseEnter;
            scrollArea.MouseExited += OnMouseExit;
        }

        foreach (Node n in GetChildren())
        {
            base.RemoveChild(n);
            this.AddChild(n);
        }

        base.AddChild(childContainer);
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

        if (@event.IsActionReleased("ScrollUp"))
        {
            CheckScrollable(true);
        }

        if (@event.IsActionReleased("ScrollDown"))
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

    public void SetResizeFormat(long value)
    {
        CheckParentContainer();
        GD.Print("resizing format: " + value.ToString());
        UpdateFormat((ResizeFormat)(int)value);
        resizeFormat = (ResizeFormat)(int)value;
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

    private void UpdateSize()
    {
        switch (resizeFormat)
        {
            case ResizeFormat.Horizontal:
                ResizeHorizontalList();
                break;
            case ResizeFormat.Vertical:
                ResizeVerticalList();
                break;
            case ResizeFormat.Grid:
                break;
            default:
                break;
        }
    }

    private void ResizeVerticalList()
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

    private void ResizeHorizontalList()
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

    private void UpdateFormat(ResizeFormat nextFormat)
    {
        switch (resizeFormat)
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

    public void SetSize(float x, float y)
    {
        if (useBounds)
        {
            Size = new Vector2(
                Math.Clamp(x, minBounds.X, maxBounds.X - padLeft - padRight),
                Math.Clamp(y, minBounds.Y, maxBounds.Y - padTop - padBot)
            );

            if (!updateParent)
                return;

            parentContainer.Size = new Vector2(
                Math.Clamp(x + padLeft + padRight, minBounds.X, maxBounds.X),
                Math.Clamp(y + padTop + padBot, minBounds.Y, maxBounds.Y)
            );
        }

        Position = new Vector2(padLeft, padTop);
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

    private void ScrollContent(bool up)
    {
        float scrollMultiplier = up ? 1 : -1;
        float scroll = scrollSpeed * scrollMultiplier;

        Vector2 scrollPos;

        switch (resizeFormat)
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

        // find bounds for scrolling
        // calculate when resizing
        // use bounds to clamp when scorlling
    }

    //
    // // following 3 functions are for finding and maintaining visible objects
    // // mostly for optimizing rather than constantly going through every object
    // private Control GetTopControl()
    // {
    //     if (children.Count < 1)
    //         return null;
    //
    //     switch (resizeFormat)
    //     {
    //         case ResizeFormat.Horizontal:
    //             for (int i = 0; i < children.Count; i++)
    //             {
    //                 Control c = children[i];
    //                 float xBound = Position.X + padLeft + padContentX - c.Size.X;
    //
    //                 if (c.Position.X >= xBound)
    //                 {
    //                     topObjIdx = i;
    //                     return c;
    //                 }
    //
    //                 c.Hide();
    //             }
    //             break;
    //         case ResizeFormat.Vertical:
    //             for (int i = 0; i < children.Count; i++)
    //             {
    //                 Control c = children[i];
    //                 float yBound = Position.Y + padTop + padContentY - c.Size.Y;
    //
    //                 if (c.Position.Y >= yBound)
    //                 {
    //                     topObjIdx = i;
    //                     return c;
    //                 }
    //
    //                 c.Hide();
    //             }
    //             break;
    //     }
    //
    //     return null;
    // }
    //
    // private void MaintainFirstObj()
    // {
    //     switch (resizeFormat)
    //     {
    //         case ResizeFormat.Horizontal:
    //             // .X => min, .Y => max
    //             Vector2 xBounds;
    //             xBounds.X = Position.X + padLeft + padContentX - firstObj.Size.X;
    //             xBounds.Y = Position.X + padLeft + padContentX;
    //
    //             if (firstObj.Position.X >= xBounds.X)
    //             {
    //                 // sort to .Count for next firstObj
    //             }
    //             else if (firstObj.Position.X <= xBounds.Y)
    //             {
    //                 // sort to 0 for next firstObj
    //             }
    //
    //             break;
    //         case ResizeFormat.Vertical:
    //             // .X => min, .Y => max
    //             Vector2 yBounds;
    //             yBounds.X = Position.Y + padTop + padContentY - firstObj.Size.Y;
    //             yBounds.Y = Position.Y + padTop + padContentY;
    //
    //             if (firstObj.Position.Y >= yBounds.X)
    //             {
    //                 // sort to .Count for next firstObj
    //             }
    //             else if (firstObj.Position.Y <= yBounds.Y)
    //             {
    //                 // sort to 0 for next firstObj;
    //             }
    //             break;
    //     }
    // }
    //
    // private void EnableObjectsInView()
    // {
    //     switch (resizeFormat)
    //     {
    //         case ResizeFormat.Horizontal:
    //             for (int i = topObjIdx + 1; i < children.Count; i++)
    //             {
    //                 Control c = children[i];
    //                 float xBound = Position.X + Size.X - padRight;
    //
    //                 if (c.Position.X <= xBound)
    //                 {
    //                     c.Show();
    //                     continue;
    //                 }
    //
    //                 break;
    //             }
    //             break;
    //         case ResizeFormat.Vertical:
    //             for (int i = 0; i < children.Count; i++)
    //             {
    //                 Control c = children[i];
    //                 float yBound = Position.Y + Size.Y - padTop;
    //
    //                 if (c.Position.Y <= yBound)
    //                 {
    //                     c.Show();
    //                     continue;
    //                 }
    //
    //                 break;
    //             }
    //             break;
    //     }
    // }

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

using System;
using Godot;

[Tool]
public partial class StaticResizeContainer : ResizingContainer
{
    public override void _Ready()
    {
        ChildEnteredTree += ChildAdded;
        ChildExitingTree += ChildRemoved;

        base._Ready();

        foreach (Node n in GetChildren())
        {
            ChildAdded(n);
        }
    }

    protected override void ChildAdded(Node node)
    {
        GD.Print("static: " + node.Name);
        if (node is Control c)
        {
            if (children.Contains(c))
                return;

            children.Add(c);

            c.Resized += UpdateSize;
        }

        UpdateSize();
    }

    protected override void ChildRemoved(Node node)
    {
        if (node is Control c)
        {
            children.Remove(c);

            c.Resized -= UpdateSize;
        }

        UpdateSize();
    }

    public override void SetResizeFormat(long value)
    {
        if (updateParent)
            CheckParentContainer();

        UpdateFormat((ResizeFormat)(int)value);
        resizeFormat = (ResizeFormat)(int)value;
    }

    protected override void ResizeVerticalList()
    {
        float y = 0f;
        float largestX = 0f;

        Vector2 pos = new Vector2(0, 0);
        if (!updateParent)
        {
            pos.Y += padTop;
            pos.X += padLeft;
        }

        foreach (Control c in children)
        {
            if (c.Size.X > largestX)
                largestX = c.Size.X;

            c.Position = new Vector2(pos.X, pos.Y + y);
            y += c.Size.Y;
            y += padContentY;
        }

        y -= padContentY;

        SetSize(largestX, y);
    }

    protected override void ResizeHorizontalList()
    {
        float x = 0f;
        float largestY = 0f;

        Vector2 pos = new Vector2(0, 0);
        if (!updateParent)
        {
            pos = new Vector2(padLeft, padTop);
        }

        foreach (Control c in children)
        {
            if (c.Size.Y > largestY)
                largestY = c.Size.Y;

            c.Position = new Vector2(pos.X + x, pos.Y);
            x += c.Size.X;
            x += padContentX;
        }

        x -= padContentX;

        SetSize(x, largestY);
    }

    public override void SetSize(float x, float y)
    {
        if (useBounds)
        {
            if (updateParent)
            {
                Size = new Vector2(
                    Math.Clamp(x, minBounds.X, maxBounds.X - padTop - padBot),
                    Math.Clamp(y, minBounds.Y, maxBounds.Y - padLeft - padRight)
                );

                Position = new Vector2(padLeft, padTop);

                parentContainer.Size = new Vector2(
                    Math.Clamp(x + padLeft + padRight, minBounds.X, maxBounds.X),
                    Math.Clamp(y + padTop + padBot, minBounds.Y, maxBounds.Y)
                );
            }
            else
            {
                Size = new Vector2(
                    Math.Clamp(x + padLeft + padRight, minBounds.X, maxBounds.X),
                    Math.Clamp(y + padTop + padBot, minBounds.Y, maxBounds.Y)
                );
            }
        }
        else
        {
            if (updateParent)
            {
                GD.Print(x, y);
                Size = new Vector2(x, y);

                Position = new Vector2(padLeft, padTop);

                parentContainer.Size = new Vector2(x + padLeft + padRight, y + padTop + padBot);
            }
            else
            {
                Size = new Vector2(x + padLeft + padRight, y + padTop + padBot);
            }
        }
    }
}

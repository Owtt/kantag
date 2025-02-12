using System;
using System.Collections.Generic;
using Godot;

// TODO: find why first spawn disappears
// TODO: create static resize container plugin

[Tool]
public abstract partial class ResizingContainer : Container
{
    public Action onResized;

    public ResizeFormat Format => resizeFormat;

    private ResizeFormat resizeFormat;

    [Export]
    protected float padTop;

    [Export]
    protected float padBot;

    [Export]
    protected float padRight;

    [Export]
    protected float padLeft;

    [Export]
    protected float padContentY;

    [Export]
    protected float padContentX;

    [Export]
    protected bool useBounds;

    [Export]
    protected bool updateParent = true;

    [Export]
    protected Vector2 minBounds;

    [Export]
    protected Vector2 maxBounds;

    protected List<Control> children = new List<Control>();

    private Control parentContainer;

    public void CheckParentContainer()
    {
        if (parentContainer == null)
            parentContainer = GetParent() as Control;
    }

    public override void _Ready()
    {
        if (updateParent)
            parentContainer = GetParent() as Control;

        foreach (Node n in GetChildren())
        {
            base.RemoveChild(n);
            this.AddChild(n);
        }
    }

    public void SetResizeFormat(long value)
    {
        if (updateParent)
            CheckParentContainer();

        UpdateFormat((ResizeFormat)(int)value);
        resizeFormat = (ResizeFormat)(int)value;
    }

    protected virtual void UpdateSize()
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

    protected abstract void ResizeVerticalList();

    protected abstract void ResizeHorizontalList();

    protected abstract void UpdateFormat(ResizeFormat nextFormat);

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
}

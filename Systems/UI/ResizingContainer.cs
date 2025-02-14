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

    protected Control parentContainer;

    public void CheckParentContainer()
    {
        if (parentContainer == null)
            parentContainer = GetParent() as Control;
    }

    public override void _Ready()
    {
        CheckParentContainer();

        UpdateSize();
    }

    protected abstract void ChildAdded(Node node);

    protected abstract void ChildRemoved(Node node);

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

    protected void UpdateFormat(ResizeFormat nextFormat)
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

    public abstract void SetSize(float x, float y);
}

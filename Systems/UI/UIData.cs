using System;
using Godot;

[GlobalClass]
public partial class UIData : Resource
{
    public Vector2 SessionScrollSize;
    public Action<Vector2> onSessionScrollSizeChanged;

    public void SetSessionScrollSize(Vector2 s)
    {
        SessionScrollSize = s;
        onSessionScrollSizeChanged?.Invoke(SessionScrollSize);
    }
}

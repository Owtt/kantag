using Godot;

public partial class UserLoader : Node
{
    [Export]
    private UserCardData _userCardData;

    public override void _Ready()
    {
        base._Ready();
        _userCardData.Initialize();
    }
}

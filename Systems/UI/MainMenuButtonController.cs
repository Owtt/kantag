using System;
using Godot;

public partial class MainMenuButtonController : Node
{
    [Export]
    private Control mainUIOptions;

    [Export]
    private Control continueButton;

    [Export]
    private Control newButton;

    private SessionView activeSessionView;

    [Export]
    private PackedScene sessionViewPrefab;

    [Export]
    private UserData userData;

    public override void _Ready()
    {
        if (userData.dataPaths.Count == 0)
        {
            continueButton.Hide();
        }
    }

    private void OnContinueUp()
    {
        GD.Print("continue up");
        mainUIOptions.Hide();
    }

    private void OnNewUp()
    {
        GD.Print("new up");
        Node c = sessionViewPrefab.Instantiate<Node>();
        activeSessionView = c as SessionView;
        GetParent().AddChild(activeSessionView);

        mainUIOptions.Hide();
    }

    private void OnLoadUp()
    {
        GD.Print("load up");
        mainUIOptions.Hide();
    }

    private void OnOptionsUp()
    {
        GD.Print("options up");
        mainUIOptions.Hide();
    }

    private void OnLoginUp()
    {
        GD.Print("login up");
        mainUIOptions.Hide();
    }

    private void OnExitUp()
    {
        GD.Print("exit up");
        GetTree().Quit();
    }
}

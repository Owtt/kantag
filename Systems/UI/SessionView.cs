using System.Collections.Generic;
using System.IO;
using Godot;

public partial class SessionView : Node
{
    public Session Session => _session;
    private Session _session;

    public Dictionary<string, CategoryView> CategoryViews = new Dictionary<string, CategoryView>();

    [Export]
    private Control categoryContainer;

    [Export]
    private PackedScene categoryPrefab;

    [Export]
    private UserData userData;

    [Export]
    private UserCardData cardData;

    public override void _Ready()
    {
        // add check for session loading
        // if first time user then do below
        foreach (string c in cardData.Categories.Keys)
        {
            Node n = categoryPrefab.Instantiate<Node>();

            categoryContainer.AddChild(n);

            CategoryView category = n as CategoryView;

            CategoryViews.Add(c, category);

            category.SetCategoryView(cardData.Categories[c]);
        }
    }
}

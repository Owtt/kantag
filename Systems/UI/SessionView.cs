using System.Collections.Generic;
using Godot;

public partial class SessionView : UIView
{
    public Session Session => _session;
    private Session _session;

    public Dictionary<string, CategoryView> CategoryViews = new Dictionary<string, CategoryView>();

    [Export]
    private UserCardData cardData;

    public override void SetView(string viewName)
    {
        // add check for session loading
        // if first time user then do below
        foreach (string c in cardData.Categories.Keys)
        {
            Node n = prefab.Instantiate<Node>();

            childContainer.AddChild(n);

            CategoryView category = n as CategoryView;

            CategoryViews.Add(c, category);

            category.SetCategoryView(cardData.Categories[c]);
        }
    }
}

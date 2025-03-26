using Godot;

public partial class CategoryView : UIView
{
    public Category Category => _category;
    private Category _category;

    public override void SetView(string viewName)
    {
        _category = cardData.CurrentSession.Categories[viewName];

        label.Text = viewName;

        foreach (string name in _category.Tags.Keys)
        {
            Node n = prefab.Instantiate<Node>();

            childContainer.AddChild(n);

            TagView tagView = n as TagView;

            tagView.SetView(name);
        }
    }
}

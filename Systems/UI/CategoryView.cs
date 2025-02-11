using Godot;

public partial class CategoryView : Node
{
    public Category Category => _category;
    private Category _category;

    [Export]
    private Label categoryLabel;

    [Export]
    private Control tagContainer;

    [Export]
    private PackedScene tagViewPrefab;

    public void SetCategoryView(Category category)
    {
        _category = category;

        categoryLabel.Text = category.Name;

        foreach (string name in category.Tags.Keys)
        {
            Node n = tagViewPrefab.Instantiate<Node>();

            tagContainer.AddChild(n);

            TagView tagView = n as TagView;

            tagView.SetTagView(category.Tags[name]);
        }
    }
}

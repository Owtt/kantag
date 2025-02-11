using System.Collections.Generic;
using Godot;

public partial class TagView : PanelContainer
{
    public Tag Tag => _tag;
    private Tag _tag;

    public Dictionary<string, CardView> cardViews = new Dictionary<string, CardView>();

    [Export]
    private Label tagTitle;

    [Export]
    private Control cardContainer;

    [Export]
    private PackedScene cardViewPrefab;

    public void SetTagView(Tag tag)
    {
        _tag = tag;

        tagTitle.Text = tag.Name;
        GD.Print(_tag.Cards.Count);

        foreach (Card c in _tag.Cards)
        {
            Node n = cardViewPrefab.Instantiate<Node>();

            cardContainer.AddChild(n);

            CardView card = n as CardView;

            card.SetCardView(c);
        }
    }
}

using Godot;

public partial class CardView : Control
{
    public Card Card => _card;
    private Card _card;

    private ResizingContainer parentContainer;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    [Export]
    private Button btn;

    [Export]
    private Label nameText;

    [Export]
    private Label dateText;

    [Export]
    private Material material;

    public void SetCardView(Card card)
    {
        _card = card;

        nameText.Text = card.Name;

        dateText.Text = card.Created.ToString("dd/mm/yy");
    }

    public void SetParent(ResizingContainer rc)
    {
        parentContainer = rc;
    }

    public override void _Ready()
    {
        parentContainer = GetParent().GetParent<ResizingContainer>();

        GuiInput += ButtonPressed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!Visible)
            GD.Print("im not visible: " + Name);
    }

    private void ButtonPressed(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            CalculateParentBounds();

            if (CheckWithinParentBounds(mb.Position))
            {
                GD.Print("Pressed");
            }
        }
    }

    private void CalculateParentBounds()
    {
        minBounds.X = parentContainer.Position.X + parentContainer.PadLeft;
        minBounds.Y = parentContainer.Position.Y + parentContainer.PadTop;

        maxBounds.X =
            parentContainer.Position.X + parentContainer.Size.X - parentContainer.PadRight;
        maxBounds.Y = parentContainer.Position.Y + parentContainer.Size.Y - parentContainer.PadBot;
    }

    private bool CheckWithinParentBounds(Vector2 pos)
    {
        bool inX = pos.X >= minBounds.X && pos.X <= maxBounds.X;
        bool inY = pos.Y >= minBounds.Y && pos.Y <= maxBounds.Y;

        return inX && inY;
    }
}

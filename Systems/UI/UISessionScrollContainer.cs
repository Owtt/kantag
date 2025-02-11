using System.Threading.Tasks;
using Godot;

public partial class UISessionScrollContainer : ScrollContainer
{
    private ScrollContainer scrollContainer;

    [Export]
    private UIData data;

    public override void _Ready()
    {
        SetSessionSize();
        // scrollContainer.Ready += SetSessionSize;
        GetTree().Root.SizeChanged += SetSessionSize;
    }

    public async void SetSessionSize()
    {
        await Task.Delay(25);
        data.SetSessionScrollSize(Size);
    }
}

namespace Neuro;

[GlobalClass]
public partial class NeuroUi : Control
{
    [Export] public Label ItemCounterLabel;

    public void UpdateCounter(int value, int max)
    {
        ItemCounterLabel.Text = $"{value}/{max}";
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Neuro;

[GlobalClass]
public partial class NeuroUi : Control
{
    [Export] public Label ItemCounterLabel;

    [Export] public AnimationPlayer Animator;

    [Export] public TextureRect ItemIcon;
    
    [Export] public Label ItemName;
    
    [Export] public Label ItemDescription;

    private List<NeuroItemEntry> _queuedItems = new();

    public override void _Process(double delta)
    {
        if (_queuedItems.Count == 0 || Animator.IsPlaying())
            return;

        NeuroItemEntry curItem = _queuedItems.First();
        _queuedItems.RemoveAt(0);
        
        ItemIcon.Texture = curItem.Icon;
        ItemName.Text = curItem.Name;
        ItemDescription.Text = curItem.Description;
        Animator.Play("open");
    }
    
    public void UpdateCounter(int value, int max)
    {
        ItemCounterLabel.Text = $"{value}/{max}";
    }

    public void ShowItem(NeuroItemEntry item)
    {
        _queuedItems.Add(item);
    }
}
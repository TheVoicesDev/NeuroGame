using System.Linq;
using Fantome;

namespace Neuro;

public partial class NeuroGame : FantomeGame
{
    [Export] public NeuroItemDatabase Items;

    [Export] public int ItemCount = 0;

    [Export] public int MaxItems = 7;

    [Export] public NeuroUi Ui;

    public override void _Ready()
    {
        base._Ready();
        
        Ui.UpdateCounter(ItemCount, MaxItems);
    }

    public void GetItem(StringName id)
    {
        NeuroItemEntry entry = Items.Entries.FirstOrDefault(x => x.Id == id);
        if (entry == null)
            return;

        ItemCount++;
        Ui.UpdateCounter(ItemCount, MaxItems);
        Ui.ShowItem(entry);
    }
}
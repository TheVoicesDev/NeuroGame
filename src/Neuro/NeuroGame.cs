using System.Linq;
using Fantome;

namespace Neuro;

[GlobalClass]
public partial class NeuroGame : FantomeGame
{
    [Export] public NeuroItemDatabase Items;

    [Export] public int ItemCount = 0;

    [Export] public int MaxItems = 7;

    [Export] public NeuroUi Ui;

    private int _lastHealth;

    public override void _Ready()
    {
        base._Ready();

        Ui.UpdateCounter(ItemCount, MaxItems);
    }

    public override void _Process(double delta)
    {
        if (_lastHealth != Player.Health)
            Ui.UpdateHealth(Player.Health / (float)Player.MaxHealth);

        _lastHealth = Player.Health;
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
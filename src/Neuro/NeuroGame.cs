using System.Linq;
using Fantome;

namespace Neuro;

public partial class NeuroGame : FantomeGame
{
    [Export] public NeuroItemDatabase Items;

    [Export] public int ItemCount = 0;

    public void GetItem(StringName id)
    {
        NeuroItemEntry entry = Items.Entries.FirstOrDefault(x => x.Id == id);
        if (entry == null)
            return;

        ItemCount++;
        
    }
}
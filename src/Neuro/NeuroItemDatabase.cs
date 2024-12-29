namespace Neuro;

[GlobalClass]
public partial class NeuroItemDatabase : Resource
{
    [Export] public NeuroItemEntry[] Entries;
}
namespace Neuro;

[GlobalClass]
public partial class NeuroItemEntry : Resource
{
    [Export] public StringName Id;

    [Export] public Texture2D Icon;

    [Export] public string Name;
    
    [Export(PropertyHint.MultilineText)] public string Description;
}
namespace Neuro;

[GlobalClass]
public partial class NeuroSequenceData : Resource
{
    [Export] public Texture2D[] Images;

    [Export] public float FrameRate;
    
    [Export] public bool Loop = false;
}
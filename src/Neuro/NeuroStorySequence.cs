namespace Neuro;

[GlobalClass]
public partial class NeuroStorySequence : Resource
{
    [Export] public NeuroSequenceData[] Data;

    [Export] public AudioStream Music;
}
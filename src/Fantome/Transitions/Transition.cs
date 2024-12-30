namespace Fantome.Transitions;

[GlobalClass]
public partial class Transition : Control
{
    [Export] public string StartingAnimation = "end";

    [Export] public string EndingAnimation = "end";
    
    [Export] public AnimationPlayer Animator;
}
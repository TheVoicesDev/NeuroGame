namespace Fantome.Characters;

[GlobalClass]
public partial class MoveSet : Node
{
    [Export] public Texture2D Image;

    [Export] public bool Active = true;

    [Export] public StateController Controller;

    [Export] public Node DefaultState;

    [Export] public Node DeathState;

    [Export] public AnimationLibrary Animations;

    [Signal] public delegate void BeginStateEventHandler(Node currentState, Node lastState);

    [Signal] public delegate void EndStateEventHandler(Node currentState, Node nextState);
}
using Fantome.Utilities;

namespace Fantome.Transitions;

[GlobalClass, StaticAutoloadSingleton("Fantome.Transitions", "TransitionManager")]
public partial class TransitionManagerInstance : CanvasLayer
{
    public Transition Current;

    public string Target;

    public Control Container;
    
    public override void _Ready()
    {
        base._Ready();

        Layer = 2;
        
        Container = new Control();
        AddChild(Container);
        
        Container.SetAnchorsPreset(Control.LayoutPreset.FullRect, true);
    }

    public void SwitchMainScene(string transition, string scene)
    {
        string transPath = "res://resources/transitions/" + transition + ".tscn";
        if (!ResourceLoader.Exists(transPath))
        {
            GetTree().ChangeSceneToFile(scene);
            return;
        }

        Target = scene;
        Current = ResourceLoader.Load<PackedScene>(transPath).Instantiate<Transition>();
        Container.AddChild(Current);
        
        Current.Animator.Play(Current.StartingAnimation);
        Current.Animator.AnimationFinished += AnimationEnded;
    }

    private void AnimationEnded(StringName name)
    {
        if (name == Current.StartingAnimation)
        {
            if (GetTree().ChangeSceneToFile(Target) == Error.Ok)
                Current.Animator.Play(Current.EndingAnimation);
        }

        if (name == Current.EndingAnimation)
        {
            Current.Animator.AnimationFinished -= AnimationEnded;
            Current.QueueFree();
        }
    }
}
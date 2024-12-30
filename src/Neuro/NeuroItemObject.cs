using System.Linq;
using Fantome;
using Fantome.Audio;

namespace Neuro;

[GlobalClass]
public partial class NeuroItemObject : Node3D
{
    [Export] public StringName Id;
    
    [Export] public AudioStream PickupSound;

    [ExportGroup("References"), Export] public Sprite3D Sprite;

    [Export] public Area3D Collision;

    private bool _retrieved = false;

    public override void _Ready()
    {
        Collision.BodyEntered += OnBodyEntered;
        
        NeuroGame game = FantomeEngine.Game as NeuroGame;
        if (game is null)
            return;

        Sprite.Texture = game.Items.Entries.First(x => x.Id == Id).Icon;
    }

    private void OnBodyEntered(Node3D body)
    {
        NeuroGame game = FantomeEngine.Game as NeuroGame;
        if (_retrieved || game == null || game.Player != body)
            return;

        if (PickupSound != null)
        {
            AudioManager.PlaySoundEffect(PickupSound);
        }
        
        _retrieved = true;
        Collision.QueueFree();
        Reparent(game.Player);

        Tween xTween = CreateTween().SetParallel();
        xTween.TweenProperty(this, "position:x", 0, 0.45).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ); 
        xTween.TweenProperty(this, "position:z", 0, 0.45).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ); 
        xTween.Play();

        Tween yTween = CreateTween().SetParallel();;
        yTween.Chain().TweenProperty(this, "position:y", 4.5, 0.15).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Circ); 
        yTween.Chain().TweenProperty(this, "position:y", 3.5, 0.25).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ);

        yTween.TweenProperty(this, "position:y", 1.5, 0.15).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ).SetDelay(0.3);
        yTween.TweenProperty(this, "scale", Vector3.Zero, 0.15).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Circ).SetDelay(0.3);

        yTween.Finished += () => game.GetItem(Id);
        yTween.Play();
    }
}
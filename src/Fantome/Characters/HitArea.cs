namespace Fantome.Characters;

[GlobalClass]
public partial class HitArea : Area3D
{
    /// <summary>
    /// The <see cref="HitBox"/> associated with this object.
    /// </summary>
    [Export] public HitBox HitBox;

    [Export] public bool Instigating = false;
    
    public override void _Ready()
    {
        base._Ready();

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    protected void OnAreaEntered(Area3D incoming)
    {
        if (incoming is not HurtBox incomingArea || incomingArea.Character == HitBox.Character || incomingArea.ActedOn.Contains(this))
            return;
        
        incomingArea.ActedOn.Add(this);
        HitBox.OnHurtEntering(this, incomingArea);
    }

    protected void OnAreaExited(Area3D exiting)
    {
        if (exiting is not HurtBox exitingArea || exitingArea.Character == HitBox.Character)
            return;

        exitingArea.ActedOn.Remove(this);
        HitBox.OnHurtExiting(this, exitingArea);
    }
}
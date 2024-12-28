namespace Fantome.Characters;

[GlobalClass]
public partial class GravityController : Node3D
{
    [Export] public bool Disabled = false;

    [Export] public float Mass = 1f;

    [Export] public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    [ExportGroup("References"), Export] public FantomeCharacter Character;
    
    private float _terminalVelocity;
    private Vector3 _lastScale = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Character.MoveAndSlide();

        if (Disabled)
            return;

        if (Character.IsOnFloor())
        {
            Vector3 charVel = Character.Velocity;
            charVel.Y = 0f;
            Character.Velocity = charVel;

            return;
        }

        Vector3 curScale = Character.GlobalBasis.Scale;
        if (curScale != _lastScale)
            UpdateTerminalVelocity();
        _lastScale = curScale;

        Vector3 charVelocity = Character.Velocity;
        float gravityDecrement = Gravity * (float)delta;
        charVelocity.Y = Mathf.Max(charVelocity.Y - gravityDecrement, -_terminalVelocity);
        Character.Velocity = charVelocity;
    }

    private void UpdateTerminalVelocity()
    {
        float density = (Mass / Character.VisualReference.GetAabb().Volume) * Character.GlobalBasis.Scale.Length();
        _terminalVelocity = Mathf.Sqrt(2 * Mass * Gravity / density);
    }
}
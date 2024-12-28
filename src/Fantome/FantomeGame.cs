using Fantome.Characters;
using Fantome.Objects;

namespace Fantome;

[GlobalClass]
public partial class FantomeGame : Node3D
{
    /// <summary>
    /// The <see cref="FantomeCharacter"/> being controlled by the player.
    /// </summary>
    [Export] public FantomeCharacter Player;

    [Export] public Node3D Focus;

    [ExportGroup("References"), Export] public CameraController Camera;

    public override void _Ready()
    {
        base._Ready();

        Focus ??= Player;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (Focus == null)
            return;
        
        Camera.TargetPosition = Focus.GlobalPosition;
    }
}
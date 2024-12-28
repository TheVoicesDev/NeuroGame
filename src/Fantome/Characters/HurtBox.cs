using Godot.Collections;

namespace Fantome.Characters;

[GlobalClass]
public partial class HurtBox : Area3D
{
    [Export] public FantomeCharacter Character;

    [Export] public Array<HitArea> ActedOn = new();
}
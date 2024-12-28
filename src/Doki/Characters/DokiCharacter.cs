using Fantome.Characters;

namespace Doki.Characters;

[GlobalClass]
public partial class DokiCharacter : FantomeCharacter
{
    [Export] public bool AttemptCombo = false;
    [Export] public bool AttemptFinisher = false;
}
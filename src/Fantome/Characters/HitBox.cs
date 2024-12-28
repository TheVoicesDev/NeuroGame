using System.Linq;
using Fantome.Utilities;
using Godot.Collections;
using Array = Godot.Collections.Array;
using SystemArray = System.Array;

namespace Fantome.Characters;

[GlobalClass]
public partial class HitBox : Node3D
{
    [Export] public FantomeCharacter Character;
    
    [Export] public bool SingleFire = true;

    [Export] public int HealthAddition = 0;

    private HitArea[] _parts;
    private Array<HurtBox> _hurtBoxesActing = new();
    private Array<HurtBox> _hurtBoxesHit = new();

    public override void _Ready() => Update();

    public override void _PhysicsProcess(double delta)
    {
        foreach (HurtBox current in _hurtBoxesActing)
        {
            if (!current.ActedOn.Any(x => x.HitBox == this && x.Instigating) || (SingleFire && _hurtBoxesHit.Contains(current)))
                continue;

            int healthAbs = Math.Abs(HealthAddition);
            if (HealthAddition <= 0)
                current.Character.Damage(healthAbs, current.Character);
            else
                current.Character.Heal(healthAbs, current.Character);
            
            if (!_hurtBoxesHit.Contains(current))
                _hurtBoxesHit.Add(current);
        }
    }
    
    public void Reset()
    {
        _hurtBoxesHit.Clear();
        HealthAddition = 0;

        for (int i = 0; i < _parts.Length; i++)
            _parts[i].Instigating = false;
    }
    
    public void Update()
    {
        Node[] children = GetChildren().ToArray();
        _parts = new HitArea[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            _parts[i] = children[i].GetChildOfType<HitArea>(recursive: true);
            _parts[i].HitBox = this;
        }

        _hurtBoxesHit.Clear();
        
        //_hurtBoxesHitting.Clear();
    }

    public int FindPart(StringName name) => SystemArray.FindIndex(_parts, x => x.Name == name);
    
    public HitArea GetPart(int idx) => _parts[idx];

    public void OnHurtEntering(HitArea self, HurtBox entering)
    {
        _hurtBoxesActing.Add(entering);
        //_hurtBoxesHitting.Add(entering);
    }

    public void OnHurtExiting(HitArea self, HurtBox exiting)
    {
        _hurtBoxesActing.Remove(exiting);
        //_hurtBoxesHitting.Remove(exiting);
    }
}
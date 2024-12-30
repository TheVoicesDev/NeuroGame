using Fantome.Audio;
using Godot.Collections;

namespace Fantome.Characters;

[GlobalClass]
public partial class FantomeCharacter : CharacterBody3D
{
	[Export] public int Health { get; set; } = 0;
	
	[Export] public int MaxHealth { get; set; } = 100;
	
	[Export] public float MoveStrength { get; set; } = 0f;
	
	[Export] public Vector3 LookDirection;

	[Export] public AudioStream HitSound;

	[ExportGroup("References"), Export] public VisualInstance3D VisualReference;

	[Export] public HitBox HitBox;
	
	[ExportSubgroup("Skeleton"), Export] public Skeleton3D SkeletonReference;
	
	[Export] public BoneAttachment3D CenterBone;
	
	[ExportSubgroup("Controllers"), Export] public StateController StateController { get; set; }

	[Export] public GravityController GravityController;

	[ExportSubgroup("Animators"), Export] public AnimationPlayer BodyAnimator { get; set; }

	[Export] public AnimationPlayer FaceAnimator { get; set; }
	
	public override void _Ready()
	{
		if (StateController == null)
		{
			GD.Print($"[FantomeCharacter] StateController is required to operate node \"{Name}\".");
			return;
		}
		
		StateController.Character = this;
		if (HitBox != null)
			HitBox.Character = this;
	}

	public void Damage(int damage, FantomeCharacter instigator)
	{
		if (damage < 0)
			return;
		
		Health -= damage; // BAD FIX LATER

		if (HitSound != null)
		{
			AudioStreamPlayer sfx = AudioManager.PlaySoundEffect(HitSound);
			sfx.VolumeDb = (float)Mathf.LinearToDb(0.4);
			sfx.PitchScale = (float)GD.RandRange(0.9f, 1.1f);
		}
		
		StateController.EmitSignal(StateController.SignalName.Damaged, damage, instigator);
	}

	public void Heal(int health, FantomeCharacter instigator)
	{
		if (health < 0)
			return;
		
		Health += health; // SAME HERE
		StateController.EmitSignal(StateController.SignalName.Healed, health, instigator);
	}
}

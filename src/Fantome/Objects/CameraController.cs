using Godot.Collections;

namespace Fantome.Objects;

[GlobalClass]
public partial class CameraController : Node3D
{
	public enum UpdateMode
	{
		None,
		Interpolation
	}

	[Export] public Vector3 TargetPosition = Vector3.Zero;
	[Export] public Vector3 OffsetPosition = Vector3.Zero;
	[Export] public Vector3 TargetRotation = Vector3.Zero;
	[Export] public Vector3 OffsetRotation = Vector3.Zero;
	[Export] public float TargetZ = 0f;
	[Export] public float OffsetZ = 0f;
	[Export(PropertyHint.Layers3DPhysics)] public uint CollisionMask = 0x0;

	[ExportGroup("Update"), Export] public UpdateMode PositionUpdate = UpdateMode.Interpolation;
	[Export] public UpdateMode RotationUpdate = UpdateMode.Interpolation;
	[Export] public UpdateMode ZUpdate = UpdateMode.Interpolation;
	[ExportSubgroup("Interpolation"), Export] public float FollowPositionWeight = 0.02f;
	[Export] public float FollowRotationWeight = 0.02f;
	[Export] public float FollowZWeight = 0.05f;

	[ExportGroup("References"), Export] public SpringArm3D SpringArm;
	[Export] public Camera3D Camera;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		UpdateRotation();
		UpdatePosition();
		UpdateZ();
	}

	protected virtual void UpdatePosition()
	{
		Vector3 targetPos = TargetPosition + OffsetPosition;
		switch (PositionUpdate)
		{
			case UpdateMode.None:
				GlobalPosition = targetPos; 
				break;
			case UpdateMode.Interpolation:
				Vector3 globalPos = GlobalPosition;
				GlobalPosition = globalPos.IsEqualApprox(targetPos) ? targetPos : globalPos.Lerp(targetPos, FollowPositionWeight);
				break;
		}
	}

	protected virtual void UpdateRotation()
	{
		Basis targetBasis = Basis.FromEuler(TargetRotation + OffsetRotation);
		switch (RotationUpdate)
		{
			case UpdateMode.None:
				GlobalBasis = targetBasis;
				break;
			case UpdateMode.Interpolation:
				Basis globalBasis = GlobalBasis;
				GlobalBasis = globalBasis.IsEqualApprox(targetBasis) ? targetBasis : globalBasis.Slerp(targetBasis, FollowRotationWeight);
				break;
		}
	}

	protected virtual void UpdateZ()
	{
		float targetZ = TargetZ + OffsetZ;
		switch (ZUpdate)
		{
			case UpdateMode.None:
				SpringArm.SpringLength = targetZ;
				break;
			case UpdateMode.Interpolation:
				float springLength = SpringArm.SpringLength;
				SpringArm.SpringLength = Mathf.IsEqualApprox(springLength, targetZ) ? targetZ : Mathf.Lerp(springLength, targetZ, FollowZWeight);
				break;
		}
	}

	public void Snap()
	{
		/*
		GlobalBasis = InternalBasis = Basis.FromEuler(TargetRotation + OffsetRotation);

		InternalPosition = TargetPosition + OffsetPosition;
		InternalZ = TargetZ + OffsetZ;

		Vector3 zVector = new Vector3(0f, 0f, InternalZ) * GlobalBasis;
		PhysicsDirectSpaceState3D space3D = GetWorld3D().DirectSpaceState;
		PhysicsRayQueryParameters3D rayCastParams = PhysicsRayQueryParameters3D.Create(InternalPosition, InternalPosition + zVector, CollisionMask);
		Dictionary results = space3D.IntersectRay(rayCastParams);
		if (results.Count > 0)
		{
			zVector = results["position"].AsVector3() - InternalPosition;
			InternalZ = zVector.Length();
		}
		GlobalPosition = InternalPosition + zVector;*/
	}
}

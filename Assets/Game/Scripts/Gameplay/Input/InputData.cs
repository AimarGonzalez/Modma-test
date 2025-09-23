
using UnityEngine;

namespace AG.Gameplay.PlayerInput
{
	public struct Movement
	{
		public float Vertical;
		public float Horizontal;
		public bool IsMoving;
		public float Angle;
		public Quaternion Rotation;

		public static Movement NoMovement = new();
	}

	public struct InputData
	{
		public Movement Movement;
		public bool IsMoving => Movement.IsMoving;
	}
}
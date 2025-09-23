

using AG.Gameplay.PlayerInput;
using SharedLib.ComponentCache;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	// Place PlayerMovement next to the CharacterController.
	public class PlayerMovement : SubComponent
	{
		[SerializeField]
		private float _speed;

		[SerializeField]
		private float _rotationLerp = 15f;

		// ------------- Components -------------
		private CharacterController _characterController;

		protected void Awake()
		{
			_characterController = Root.Get<CharacterController>();
		}

		public void Move(InputData inputData)
		{
			UpdatePosition(inputData.Movement.Horizontal, inputData.Movement.Vertical);
			UpdateRotation(inputData.Movement.Angle);
		}

		private void UpdatePosition(float movX, float movY)
		{
			Vector3 movement = new(movX * _speed * Time.deltaTime, 0, movY * _speed * Time.deltaTime);
			_characterController.Move(movement);
		}

		private void UpdateRotation(float angle)
		{
			// Convert from cartesian angles to transform Y angles. (Y angles start north and go clockwise)
			float rotationY = 90 - angle * Mathf.Rad2Deg;

			Quaternion targetRotation = Quaternion.AngleAxis(rotationY, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationLerp * Time.deltaTime);
		}
	}
}


using AG.Gameplay.PlayerInput;
using SharedLib.ComponentCache;
using UnityEngine;
using UnityEngine.Serialization;

namespace AG.Gameplay.Characters.Components
{
	// Place PlayerMovement next to the CharacterController.
	public class PlayerMovement : SubComponent
	{
		[SerializeField]
		private float _speed;

		[FormerlySerializedAs("_rotationLerp")] [SerializeField]
		private float _rotationSpeedWalking = 15f;
		
		private float _rotationSpeedAttacking = 60f;

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

		public void LookAt(Transform target)
		{
			if(target == null)
			{
				return;
			}
			
			// Direction
			Vector3 direction = target.position - RootTransform.position;
			direction.y = 0;
			direction.Normalize();
			
			Quaternion targetRotation =  Quaternion.LookRotation(direction);
			RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, _rotationSpeedAttacking * Time.deltaTime);
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
			RootTransform.rotation = Quaternion.Slerp(RootTransform.rotation, targetRotation, _rotationSpeedWalking * Time.deltaTime);
		}
	}
}
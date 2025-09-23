using AG.Gameplay.UI;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.PlayerInput
{
	public class PlayerInputController : SubComponent
	{
		[SerializeField] bool _constantSpeed = true;

		// ------------- Dependencies -------------
		[Inject]
		private UIProvider _uiProvider;
		
		private Joystick _joystick;

		// ------------- Private fields -------------

		private InputData _inputData;

		// ------------- Public properties -------------

		public InputData InputData => _inputData;

		private void Awake()
		{
			_joystick = _uiProvider.Joystick;
		}

		private void Update()
		{
			float joystickX = _joystick.Horizontal;
			float joystickY = _joystick.Vertical;

#if UNITY_EDITOR
			// If the joystick is not being used, read the keyboard input
			if (joystickX == 0 && joystickY == 0)
			{
				joystickX = Input.GetAxisRaw("Horizontal");
				joystickY = Input.GetAxisRaw("Vertical");
			}
#endif

			if (Math.Abs(joystickX) > 0 || Math.Abs(joystickY) > 0)
			{
				float angle = Mathf.Atan2(joystickY, joystickX);

				float movX = joystickX;
				float movY = joystickY;

				if (_constantSpeed)
				{
					movX = Mathf.Cos(angle);
					movY = Mathf.Sin(angle);
				}

				// The joystick angle is in cartesian coordinaes. But in transforms, Y rotation starts north and goes clockwise
				// Convert from cartesian angles to transform's Y angles.
				float rotationY = 90 - angle * Mathf.Rad2Deg;

				Quaternion rotation = Quaternion.AngleAxis(rotationY, Vector3.up);

				_inputData.Movement.Vertical = movY;
				_inputData.Movement.Horizontal = movX;
				_inputData.Movement.Angle = angle;
				_inputData.Movement.Rotation = rotation;
				_inputData.Movement.IsMoving = true;
			}
			else
			{
				_inputData.Movement = Movement.NoMovement;
			}
		}
	}
}
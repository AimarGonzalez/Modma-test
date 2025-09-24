using AG.Gameplay.UI;
using SharedLib.ComponentCache;
using UnityEngine;
using VContainer;

namespace Modma
{
    public class JoystickFollower : SubComponent
    {
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private float _maxRadius = 1f;

		// ------------- Dependencies -------------
		[Inject]
		private UIProvider _uiProvider;
		private Joystick _joystick;

		private void Awake()
		{
			_joystick = _uiProvider.Joystick;
		}

		void Update()
		{
			if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
			{
				_target.gameObject.SetActive(true);
				_target.position = RootTransform.position + new Vector3(_joystick.Horizontal * _maxRadius, 0, _joystick.Vertical * _maxRadius);
			}
			else
			{
				_target.gameObject.SetActive(false);
			}
        }
    }
}

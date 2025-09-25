using UnityEngine;

namespace AG.Gameplay.UI
{
	public class UIProvider : MonoBehaviour
	{
		[SerializeField] private Joystick _joystick;
		[SerializeField] private Canvas _healthBarsCanvas;
		[SerializeField] private Camera _worldCamera;

		public Joystick Joystick => _joystick;
		public Canvas HealthBarsCanvas => _healthBarsCanvas;
		public Camera WorldCamera => _worldCamera;
	}
}
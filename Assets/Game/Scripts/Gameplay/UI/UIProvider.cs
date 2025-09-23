using UnityEngine;

namespace AG.Gameplay.UI
{
    public class UIProvider : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        public Joystick Joystick => _joystick;
    }
}
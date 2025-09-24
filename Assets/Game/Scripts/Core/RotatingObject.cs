using UnityEngine;

namespace AG.Core
{
	public class RotatingObject : MonoBehaviour
	{
		[SerializeField] private float _rotationSpeedX;
		[SerializeField] private float _rotationSpeedY;
		[SerializeField] private float _rotationSpeedZ;

		private void Update()
		{
			transform.Rotate(_rotationSpeedX * Time.deltaTime, _rotationSpeedY * Time.deltaTime, _rotationSpeedZ * Time.deltaTime);
		}
	}
}
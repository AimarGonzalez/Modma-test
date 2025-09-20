using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLib.Utils
{
	// This class is used to transfer its position to parent or sibling game object.
	// Meant to be used with a Collider, CharacterController, Rigidbody or Animator with root motion.
	public class TransferMovement : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		[InfoBox("Use FixedUpdate for Rigidbodies, Colliders or CharacterController.\n" +
		"Use Update for Animator.\n" +
		"Use LateUpdate for other regular components.")]
		[SerializeField]
		private UpdateMode _mode = UpdateMode.FixedUpdate;

		private void Update()
		{
			if (_mode == UpdateMode.Update)
			{
				Transfer();
			}
		}

		private void LateUpdate()
		{
			if (_mode == UpdateMode.LateUpdate)
			{
				Transfer();
			}
		}

		private void FixedUpdate()
		{
			if (_mode == UpdateMode.FixedUpdate)
			{
				Transfer();
			}
		}

		private void Transfer()
		{
			// move to bodies position
			_target.position = transform.position;

			if (transform.IsChildOf(_target))
			{
				// clear local position to keep same world position
				transform.localPosition = Vector3.zero;
			}
		}
	}
}
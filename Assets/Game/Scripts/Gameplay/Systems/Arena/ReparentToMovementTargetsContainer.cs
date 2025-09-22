using AG.Gameplay.Combat;
using UnityEngine;

namespace AG.Gameplay.Systems.Arena
{
	public class ReparentToMovementTargetsContainer : MonoBehaviour
	{
		// ------------ Dependencies ----------------
		private ArenaWorld _arenaWorld;

		// ------------ Private fields ----------------
		private Transform _movementTargetsContainer;
		private Transform _originalParent;

		private void Awake()
		{
			_arenaWorld = FindFirstObjectByType<ArenaWorld>();
			_movementTargetsContainer = _arenaWorld.MovementTargetsContainer;

			_originalParent = transform.parent;
		}

		private void OnEnable()
		{
			transform.SetParent(_movementTargetsContainer.transform);
		}

		private void OnDisable()
		{
			transform.SetParent(_originalParent);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}
}
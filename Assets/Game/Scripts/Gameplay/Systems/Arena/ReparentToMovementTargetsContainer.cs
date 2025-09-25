using AG.Core.Pool;
using AG.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Systems.Arena
{
	public class ReparentToMovementTargetsContainer : MonoBehaviour, IPooledComponent
	{
		// ------------ Dependencies ----------------
		[Inject]
		private GameplayWorld _gameplayWorld;

		// ------------ Private fields ----------------
		private Transform _movementTargetsContainer;
		private Transform _originalParent;

		private void Awake()
		{
			FetchDependencies();
		}

		private void FetchDependencies()
		{
			_movementTargetsContainer ??= _gameplayWorld.MovementTargetsContainer;
			_originalParent ??= transform.parent;
		}

		public void OnBeforeGetFromPool()
		{
			FetchDependencies();
		}

		public void OnAfterGetFromPool()
		{
			transform.SetParent(_movementTargetsContainer.transform);
		}

		public void OnReturnToPool()
		{
			transform.SetParent(_originalParent);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}

		public void OnDestroyFromPool()
		{
		}
	}
}
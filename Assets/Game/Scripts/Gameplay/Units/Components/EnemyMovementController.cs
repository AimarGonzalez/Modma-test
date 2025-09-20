

using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Units.Components
{
	public class EnemyMovementController : SubComponent
	{

		private enum EnemyState
		{
			Idle,
			Patrolling,
			Attacking,
			Dead
		}

		// ------------- Inspector fields -------------

		[SerializeField]
		private FlagSO[] _blockingFlags;

		// ------------- Components -------------

		[SerializeField]
		private Flags _flags;

		//-------------- Private fields --------------

		[ShowInInspector]
		private EnemyState _state;

		private bool IsBlocked => _flags.IsAnyFlagActive(_blockingFlags);


		private void Update()
		{
			if (IsBlocked)
			{
				return;
			}

			switch (_state)
			{
				case EnemyState.Idle:
					SetState(EnemyState.Patrolling);
					break;
				case EnemyState.Patrolling:
					Move();
					break;
				case EnemyState.Attacking:
					Attack();
					break;
				case EnemyState.Dead:
					break;
			}
		}

		private void SetState(EnemyState state)
		{
			// Exit current state
			switch (_state)
			{
				case EnemyState.Idle:
					break;
			}

			_state = state;

			// Enter new state
			switch (state)
			{
				case EnemyState.Idle:
					break;
			}
		}

		private void Move()
		{
			// TODO: Play an action
		}

		private void Attack()
		{
			// TODO: Play an action
		}

	}
}
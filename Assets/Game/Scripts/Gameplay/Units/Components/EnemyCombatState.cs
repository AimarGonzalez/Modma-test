using AG.Gameplay.Actions;
using AG.Gameplay.Characters;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Units.Components
{
	public enum EnemyState
	{
		Idle,
		Moving,
		Attacking
	}
	
	public class EnemyCombatState : StateSubComponent
	{
		// ------------- Inspector fields -------------
		
		[SerializeField]
		private ActionId _moveActionId;

		[SerializeField]
		private ActionId _attackActionId;
		
		[SerializeField]
		private FlagSO[] _blockingFlags;

		// ------------- Components -------------

		private Character _character;
		private Flags _flags;
		private ActionPlayer _actionPlayer;

		//-------------- Private fields --------------

		[ShowInInspector, ReadOnly]
		private EnemyState _subState;

		private bool IsBlockedByActions => _flags.IsAnyFlagActive(_blockingFlags);
		public EnemyState State  => _subState;


		protected void Awake()
		{
			_character = Root.Get<Character>();
			_flags = Root.Get<Flags>();
			_actionPlayer = Root.Get<ActionPlayer>();
		}
		
		public override void EnterState()
		{
			_character.OnHitReceived += OnHitReceivedHandler;
		}
		public override void ExitState()
		{
			_character.OnHitReceived -= OnHitReceivedHandler;
		}

		public override IState.Status UpdateState()
		{
			if (IsBlockedByActions)
			{
				return IState.Status.Running;
			}

			switch (_subState)
			{
				case EnemyState.Idle:
					SetSubState(EnemyState.Moving);
					break;
				case EnemyState.Moving:
					Move();
					break;
				case EnemyState.Attacking:
					Attack();
					break;
			}
			
			return IState.Status.Running;
		}

		private void SetSubState(EnemyState state)
		{
			_subState = state;
		}

		private void Move()
		{
			_actionPlayer.TryPlayAction(_moveActionId, onFinished: OnMoveFinished);
		}

		private void OnMoveFinished()
		{
			SetSubState(EnemyState.Attacking);
		}

		private void Attack()
		{
			_actionPlayer.TryPlayAction(_attackActionId, onFinished: OnAttackFinished);
		}

		private void OnAttackFinished()
		{
			SetSubState(EnemyState.Moving);
		}

		private void OnHitReceivedHandler(Character source, float damage)
		{
			_character.Health -= damage;
		}
	}
}
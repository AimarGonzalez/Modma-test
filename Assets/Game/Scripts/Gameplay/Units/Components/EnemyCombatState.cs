using AG.Gameplay.Actions;
using AG.Gameplay.Characters;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Units.Components
{
	
	public class EnemyCombatState : StateSubComponent
	{
		public enum CombatState
		{
			Idle,
			Moving,
			Attacking
		}
		
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
		private CombatState _combatState;

		private bool IsBlockedByActions => _flags.IsAnyFlagActive(_blockingFlags);
		public CombatState State  => _combatState;


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

			switch (_combatState)
			{
				case CombatState.Idle:
					SetSubState(CombatState.Moving);
					break;
				case CombatState.Moving:
					Move();
					break;
				case CombatState.Attacking:
					Attack();
					break;
			}
			
			return IState.Status.Running;
		}

		private void SetSubState(CombatState state)
		{
			_combatState = state;
		}

		private void Move()
		{
			_actionPlayer.TryPlayAction(_moveActionId, onFinished: OnMoveFinished);
		}

		private void OnMoveFinished()
		{
			SetSubState(CombatState.Attacking);
		}

		private void Attack()
		{
			_actionPlayer.TryPlayAction(_attackActionId, onFinished: OnAttackFinished);
		}

		private void OnAttackFinished()
		{
			SetSubState(CombatState.Moving);
		}

		private void OnHitReceivedHandler(Character source, float damage)
		{
			_character.Health -= damage;
		}
	}
}
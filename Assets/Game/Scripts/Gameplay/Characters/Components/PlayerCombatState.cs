using AG.Core.UI;
using AG.Gameplay.Actions;
using AG.Gameplay.PlayerInput;
using AG.Gameplay.Systems;
using SharedLib.StateMachines;
using SharedLib.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerCombatState : StateSubComponent, IDebugPanelDrawer
	{
		public enum CombatState
		{
			Moving,
			Attacking
		}

		// ------------- Inspector fields -------------

		[SerializeField]
		private ActionId _attackActionId;

		[FormerlySerializedAs("_blockingFlags")] [SerializeField]
		private FlagSO[] _blockInputFlags;


		// ------------- Private fields -------------

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private CombatState _state;

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private float _attackSpeed;
		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Timer _rangedAttackTimer;
		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Timer _dashAttackTimer;
		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Character _target;

		// ------------- Public properties -------------
		public bool HasTarget => _target != null;

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		public float RangedAttackCooldown => _rangedAttackTimer?.TimeLeft ?? 0f;
		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		public float DashAttackCooldown => _dashAttackTimer?.TimeLeft ?? 0f;

		// ------------- Components -------------
		private PlayerInputController _inputController;
		private PlayerMovement _playerMovement;
		private PlayerAnimations _playerAnimations;


		[Inject]
		private ArenaEvents _arenaEvents;

		protected void Awake()
		{
			_rangedAttackTimer = new Timer();
			_dashAttackTimer = new Timer();

			_inputController = Root.Get<PlayerInputController>();
			_playerMovement = Root.Get<PlayerMovement>();
			_playerAnimations = Root.Get<PlayerAnimations>();
		}

		public override void OnEnterState()
		{
			Subscribe();
		}
		public override void OnExitState()
		{
			Unsubscribe();
		}

		private void Subscribe()
		{
		}

		private void Unsubscribe()
		{
		}

		public override IState.Status UpdateState()
		{
			_rangedAttackTimer.Elapsed(Time.deltaTime);
			_dashAttackTimer.Elapsed(Time.deltaTime);

			switch (_state)
			{
				case CombatState.Moving:
					UpdateMovement();
					break;
				case CombatState.Attacking:
					// wait
					break;
			}

			return IState.Status.Running;
		}

		private void UpdateMovement()
		{
			if (_inputController.InputData.IsMoving)
			{
				_playerAnimations.PlayMove();
				_playerMovement.Move(_inputController.InputData);
			}
			else
			{
				_playerAnimations.PlayIdle();
			}
		}

		public void Attack()
		{
		}

		private void OnAnimationHitEvent()
		{
			// TODO: Move to Action so we can differentiate between mele and ranged actions. 
			/*
			if (Character.CombatStats.IsMelee)
			{
				_arenaEvents.TriggerCharacterAttacked(Character, _target);
			}
			else
			{
				_arenaEvents.TriggerProjectileFired(Character, _target);
			}
			*/
		}

		private void StartCooldown()
		{
			Debug.LogError("Cooldown not implemented");
		}

		void IDebugPanelDrawer.AddDebugProperties(List<GUIUtils.Property> properties)
		{
			properties.Add(new GUIUtils.Property("Combat state", _state.ToString()));
		}
	}
}


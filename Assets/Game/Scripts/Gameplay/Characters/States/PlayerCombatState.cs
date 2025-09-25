using AG.Core.UI;
using AG.Gameplay.Actions;
using AG.Gameplay.Cards.CardStats;
using AG.Gameplay.Combat;
using AG.Gameplay.PlayerInput;
using AG.Gameplay.Systems;
using SharedLib.StateMachines;
using SharedLib.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerCombatState : StateSubComponent, IDebugPanelDrawer
	{
		public enum CombatState
		{
			AutoAttacking,
			SpecialAttack
		}

		// ------------- Events -------------
		public event Action<Character> OnTargetChanged;

		// ------------- Components -------------
		private ActionPlayer _actionPlayer;

		// ------------- Inspector fields -------------

		[SerializeField]
		private ActionId _attackActionId;

		[SerializeField]
		private FlagSO[] _blockInputFlags;

		[SerializeField]
		private AttackStatsSO _rangedAttackStats;

		[SerializeField]
		private AttackStatsSO _dashAttackStats;


		// ------------- Private fields -------------

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private CombatState _state;

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Timer _rangedAttackCooldown;

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Timer _dashAttackCooldown;

		[ShowInInspector, ReadOnly, FoldoutGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Character _target;

		private IActionStatus _attackActionStatus;

		// ------------- Public properties -------------
		public bool HasTarget => _target != null;

		// ------------- Components -------------
		private PlayerInputController _inputController;
		private PlayerMovement _playerMovement;
		private PlayerAnimations _playerAnimations;

		// ------------- Dependencies -------------
		[Inject] private GameplayWorld _world;
		[Inject] private ArenaEvents _arenaEvents;

		protected void Awake()
		{
			_rangedAttackCooldown = new Timer(_rangedAttackStats.Cooldown);
			_dashAttackCooldown = new Timer(_dashAttackStats.Cooldown);

			_inputController = Root.Get<PlayerInputController>();
			_playerMovement = Root.Get<PlayerMovement>();
			_playerAnimations = Root.Get<PlayerAnimations>();
			_actionPlayer = Root.Get<ActionPlayer>();
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
			_rangedAttackCooldown.Elapsed(Time.deltaTime);
			_dashAttackCooldown.Elapsed(Time.deltaTime);

			UpdateTarget();

			switch (_state)
			{
				case CombatState.AutoAttacking:
					if (_inputController.InputData.IsMoving)
					{
						Move();
					}
					else
					{
						AutoAttack();
					}
					break;

				case CombatState.SpecialAttack:
					// wait
					break;
			}

			return IState.Status.Running;
		}

		private void AutoAttack()
		{
			if (!_target)
			{
				_playerAnimations.PlayRelaxedIdle();
				return;
			}
			
			_playerMovement.LookAt(_target);

			if (_attackActionStatus != null)
			{
				return;
			}

			if (_rangedAttackCooldown.IsRunning)
			{
				_playerAnimations.PlayAimingIdle();
				return;
			}

			_attackActionStatus = _actionPlayer.PlayAction(_attackActionId, _target, onFinished: OnAttackFinished);
			if (_attackActionStatus != null && _attackActionStatus.Status == ActionStatus.Running)
			{
				_rangedAttackCooldown.Restart();
			}
		}

		private void Move()
		{
			_playerAnimations.PlayMove();
			_playerMovement.Move(_inputController.InputData);

			if (_attackActionStatus != null)
			{
				_actionPlayer.StopAction(_attackActionStatus);
			}
		}

		private void OnAttackFinished(ActionStatus _)
		{
			_attackActionStatus = null;
		}

		void IDebugPanelDrawer.AddDebugProperties(List<GUIUtils.Property> properties)
		{
			properties.Add(new GUIUtils.Property("Combat state", _state.ToString()));
		}

		private void UpdateTarget()
		{
			Character closestEnemy = _world.GetClosestEnemy(RootTransform.position);
			SetTarget(closestEnemy);
		}

		private void SetTarget(Character target)
		{
			if (_target == target)
			{

			}

			_target = target;
			OnTargetChanged?.Invoke(_target);
		}
	}
}


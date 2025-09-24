using AG.Gameplay.Characters.Components;
using Animancer;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class PlayerRangedAutoAttackAction : BaseAction
	{

		// ------------- Inspector fields -------------
		[SerializeField] private StringAsset _shootEvent;
		[SerializeField] private StringAsset _shootEndEvent;

		// ------------- Components -------------
		private AnimancerComponent _animancer;
		private PlayerAnimations _playerAnimations;

		// ------------- Private fields -------------

		protected override void Awake()
		{
			_playerAnimations = Root.Get<PlayerAnimations>();
			_animancer = Root.Get<AnimancerComponent>();
		}

		protected override void DoStartAction(object parameters)
		{
			AnimancerState state = _playerAnimations.PlayRangedAttack();

			Subscribe();
		}

		private void Subscribe()
		{
			_animancer.Events.AddTo(_shootEvent, OnShoot);
			_animancer.Events.AddTo(_shootEndEvent, OnShootEnd);
		}

		private void Unsubscribe()
		{
			_animancer.Events.Remove(_shootEvent, OnShoot);
			_animancer.Events.Remove(_shootEndEvent, OnShootEnd);
		}

		private void OnShoot()
		{
			//Root.Get<ArenaEvents>().TriggerProjectileFired(Root.Get<Character>(), Root.Get<Character>());

			//TODO
			// - do damage
			// - play vfx
			// - play sound
		}

		private void OnShootEnd()
		{
			_playerAnimations.PlayAimingIdle();
			Status = ActionStatus.Finished;
		}

		protected override void DoOnActionFinished()
		{
			Unsubscribe();
		}

		protected override void DoInterruptAction()
		{
			Unsubscribe();
		}
	}
}
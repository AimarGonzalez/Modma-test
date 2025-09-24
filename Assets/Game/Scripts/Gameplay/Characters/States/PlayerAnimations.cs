using Animancer;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerAnimations : SubComponent
	{
		// --------- Inspector fields ---------
		
		[SerializeField, InlineEditor] private TransitionAsset _idleAnimation;
		[SerializeField, InlineEditor] private TransitionAsset _aimingIdle;
		[SerializeField, InlineEditor] private TransitionAsset _moveAnimation;
		[SerializeField, InlineEditor] private TransitionAsset _rangedAttackAnimation;
		[SerializeField, InlineEditor] private TransitionAsset _dashAttackAnimation;


		// --------- Private fields ---------
		private AnimancerComponent _animancer;

		protected void Awake()
		{
			_animancer = Root.Get<AnimancerComponent>();
		}
		
		public AnimancerState PlayIdle()
		{
			return _animancer.Play(_aimingIdle);
		}

		public AnimancerState PlayAimingIdle()
		{
			return _animancer.Play(_aimingIdle);
		}

		public AnimancerState PlayMove()
		{
			return _animancer.Play(_moveAnimation);
		}

		public AnimancerState PlayRangedAttack()
		{
			return _animancer.Play(_rangedAttackAnimation);
		}

		public AnimancerState PlayDashAttack()
		{
			return _animancer.Play(_dashAttackAnimation);
		}
	}
}
using Animancer;
using SharedLib.ComponentCache;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerAnimations : SubComponent
	{
		// --------- Inspector fields ---------
		[SerializeField] private ClipTransition _idleAnimation;
		[SerializeField] private ClipTransition _moveAnimation;
		[SerializeField] private ClipTransition _rangedAttackAnimation;
		[SerializeField] private ClipTransition _dashAttackAnimation;


		// --------- Public properties ---------
		public ClipTransition IdleAnimation => _idleAnimation;
		public ClipTransition MoveAnimation => _moveAnimation;
		public ClipTransition RangedAttackAnimation => _rangedAttackAnimation;
		public ClipTransition DashAttackAnimation => _dashAttackAnimation;

		// --------- Private fields ---------
		private AnimancerComponent _animancer;

		protected void Awake()
		{
			_animancer = Root.Get<AnimancerComponent>();
		}

		public void PlayIdle()
		{
			_animancer.Play(_idleAnimation);
		}

		public void PlayMove()
		{
			_animancer.Play(_moveAnimation);
		}

		public void PlayRangedAttack()
		{
			_animancer.Play(_rangedAttackAnimation);
		}

		public void PlayDashAttack()
		{
			_animancer.Play(_dashAttackAnimation);
		}
	}
}
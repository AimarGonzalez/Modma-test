using AG.Gameplay.Systems;
using Animancer;
using SharedLib.StateMachines;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerDeathState : StateSubComponent
	{
		// ------ Components ------

		private PlayerAnimations _playerAnimation;
		private Character _character;

		// ------------- Private fields -------------
		private AnimancerEvent.Sequence _animationEvents;

		protected void Awake()
		{
			_playerAnimation = Root.Get<PlayerAnimations>();
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			_animationEvents = _playerAnimation.PlayDeath().Events(this);
			_animationEvents.OnEnd = OnAnimationEnd;
		}

		private void OnAnimationEnd()
		{
			_animationEvents.OnEnd = null;
			
			// Will trigger ArenaEvent.TriggerCharacterRemoved
			_character.ReleaseToPool();
		}

		public override void OnExitState()
		{
		}

		public override IState.Status UpdateState()
		{
			return IState.Status.Running;
		}
	}
}

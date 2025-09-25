using AG.Gameplay.Systems;
using Animancer;
using SharedLib.StateMachines;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerDeathState : StateSubComponent
	{
		// ------------- Inspector fields -------------

		[SerializeField]
		private TransitionAsset _deathAnimation;

		// ------ Components ------

		private AnimancerComponent _animancer;
		private Character _character;

		// ------------- Dependencies -------------

		protected void Awake()
		{
			_animancer = Root.Get<AnimancerComponent>();
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			_animancer.Play(_deathAnimation);

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

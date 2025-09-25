using AG.Gameplay.Systems;
using Animancer;
using MoreMountains.Feedbacks;
using SharedLib.ExtensionMethods;
using SharedLib.StateMachines;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class EnemyDeathState : StateSubComponent
	{
		// ------------- Inspector fields -------------
		
		[SerializeField]
		private MMF_Player _deathFeedback;

		[SerializeField]
		private TransitionAsset _deathAnimation;

		// ------ Components ------

		private Character _character;

		// ------------- Dependencies -------------

		[Inject] private ArenaEvents _arenaEvents;

		protected void Awake()
		{
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			PlayDeathFeedback().RunAsync();
		}

		private async Task PlayDeathFeedback()
		{
			await _deathFeedback.PlayFeedbacksTask();

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
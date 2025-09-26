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
		private Transform _viewTransform;

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
			_deathFeedback.Initialization();
			await _deathFeedback.PlayFeedbacksTask();

			// Manually reset position - feedback not reseting as expected
			_viewTransform.localScale = Vector3.one;
			_viewTransform.localRotation = Quaternion.identity;
			_viewTransform.localPosition = Vector3.zero;
			
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
using MoreMountains.Feedbacks;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	public class SpawningState : StateSubComponent
	{
		// ------------- Inspector fields -------------
	
		[SerializeField, Required]
		private MMF_Player _spawnningFeedbacks;

		// ------------- Private fields -------------

		private IState.Status _status = IState.Status.Running;

		public override void OnEnterState()
		{
			_spawnningFeedbacks.PlayFeedbacks();

			_spawnningFeedbacks.Events.OnComplete.AddListener(OnSpawnningFeedbacksComplete);
		}

		private void OnSpawnningFeedbacksComplete()
		{
			_status = IState.Status.Finished;
		}

		public override IState.Status UpdateState()
		{
			return _status;
		}

		public override void OnExitState()
		{
			_spawnningFeedbacks.Events.OnComplete.RemoveListener(OnSpawnningFeedbacksComplete);
		}
	}
}
using MoreMountains.Feedbacks;
using SharedLib.StateMachines;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	public class SpawningState : StateSubComponent
	{
		// ------------- Inspector fields -------------
	
		[SerializeField]
		private MMF_Player _spawnningFeedbacks;

		// ------------- Private fields -------------

		private IState.Status _status = IState.Status.Running;
		
		private Character _character;

		private void Awake()
		{
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			if (_spawnningFeedbacks)
			{
				_spawnningFeedbacks.PlayFeedbacks();
				_spawnningFeedbacks.Events.OnComplete.AddListener(OnSpawnningFeedbacksComplete);
			}
		}

		public override IState.Status UpdateState()
		{
			return _status;
		}

		public override void OnExitState()
		{
			if (_spawnningFeedbacks)
			{
				_spawnningFeedbacks.Events.OnComplete.RemoveListener(OnSpawnningFeedbacksComplete);
			}
		}

		private void OnSpawnningFeedbacksComplete()
		{
			_character.Cinematic();
			_status = IState.Status.Finished;
		}
	}
}
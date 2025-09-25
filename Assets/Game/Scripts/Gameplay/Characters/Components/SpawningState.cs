using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using MoreMountains.Feedbacks;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class SpawningState : StateSubComponent
	{
		// ------------- Inspector fields -------------
	
		[SerializeField]
		private MMF_Player _spawnningFeedbacks;

		// ------------- Dependencies -------------

		[Inject] private ApplicationEvents _appEvents;
		
		// ------------- Private fields -------------

		private IState.Status _status = IState.Status.Running;
		
		private Character _character;

		private void Awake()
		{
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			_appEvents.OnAppStateChanged += OnAppStateChanged;

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
			_appEvents.OnAppStateChanged += OnAppStateChanged;
			
			if (_spawnningFeedbacks)
			{
				_spawnningFeedbacks.Events.OnComplete.RemoveListener(OnSpawnningFeedbacksComplete);
			}
		}
		
		private void OnAppStateChanged(AppState oldAppState, AppState newAppState)
		{
			switch (newAppState)
			{
				case AppState.Battle:
					_character.Fight();
					break;
				case AppState.None:
				case AppState.Welcome:
				case AppState.BattleIntro:
				case AppState.BattlePaused:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(newAppState), newAppState, null);
			}
		}

		private void OnSpawnningFeedbacksComplete()
		{
			_status = IState.Status.Finished;
		}
	}
}
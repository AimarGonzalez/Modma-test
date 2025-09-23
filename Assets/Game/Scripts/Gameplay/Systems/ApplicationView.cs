using AG.Gameplay.Combat;
using MoreMountains.Feedbacks;
using SharedLib.ExtensionMethods;
using SharedLib.Utils;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Modma.Game.Scripts.Gameplay.Systems
{
	public class ApplicationView : MonoBehaviour
	{
		[Serializable]
		private class ApplicationTransitionDictionary : UnitySerializedDictionary<AppState, MMF_Player> { }

		// ------------- Inspector fields -------------

		private MMF_Player _appStartSetupUI;
		[SerializeField] private ApplicationTransitionDictionary _enterStateTransitions = new();
		[SerializeField] private ApplicationTransitionDictionary _exitStateTransitions = new();

		// ------------- Private fields -------------

		private bool _playingTransition = false;

		// ------------- Public properties -------------
		public bool IsPlayingTransition => _playingTransition;

		public void SetupUIAtGameStart()
		{
			_appStartSetupUI.PlayFeedbacks();
		}

		public async Task PlayViewTransition(AppState oldState, AppState newState)
		{
			_playingTransition = true;

			Task[] paralelTasks =
			{
				PlayEnterStateTransition(newState),
				PlayExitStateTransition(oldState)
			};

			await Task.WhenAll(paralelTasks);

			_playingTransition = false;
		}

		private async Task PlayEnterStateTransition(AppState newState)
		{
			if (_enterStateTransitions.TryGetValue(newState, out var enterTransition))
			{
				await enterTransition.PlayFeedbacksTask();
			}
			else if (_exitStateTransitions.TryGetValue(newState, out var exitTransition))
			{
				await exitTransition.PlayFeedbacksTask(Vector3.zero, feedbacksIntensity: 1f, forceChangeDirection: true);
			}
		}

		private async Task PlayExitStateTransition(AppState oldState)
		{
			if (_exitStateTransitions.TryGetValue(oldState, out var exitTransition))
			{
				await exitTransition.PlayFeedbacksTask();
			}
			else if (_enterStateTransitions.TryGetValue(oldState, out var enterTransition))
			{
				await enterTransition.PlayFeedbacksTask(Vector3.zero, feedbacksIntensity: 1f, forceChangeDirection: true);
			}
		}
	}
}
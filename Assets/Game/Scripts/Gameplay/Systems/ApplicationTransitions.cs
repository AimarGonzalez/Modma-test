using AG.Gameplay.Combat;
using MoreMountains.Feedbacks;
using SharedLib.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Modma.Game.Scripts.Gameplay.Systems
{
	public class ApplicationTransitions : MonoBehaviour
	{
		[Serializable]
		private class ApplicationTransitionDictionary : UnitySerializedDictionary<AppState, MMF_Player>
		{
		}

		// ------------- Inspector fields -------------

		[SerializeField] private ApplicationTransitionDictionary _enterStateTransitions = new();
		[SerializeField] private ApplicationTransitionDictionary _exitStateTransitions = new();

		public async Task PlayEnterStateTransition(AppState newState)
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

		public async Task PlayExitStateTransition(AppState oldState)
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
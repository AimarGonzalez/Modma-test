using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using AG.Gameplay.Units.MonoBehaviours.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace AG.Gameplay.UI
{
	public class GameStateUIView : UIView<ApplicationFlow.State>
	{
		[Inject]
		private ApplicationEvents _appEvents;

		private void Awake()
		{
			_appEvents.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnDestroy()
		{
			_appEvents.OnGameStateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(ApplicationFlow.State oldState, ApplicationFlow.State newState)
		{
			base.OnStateChanged(oldState, newState);
		}
	}
}

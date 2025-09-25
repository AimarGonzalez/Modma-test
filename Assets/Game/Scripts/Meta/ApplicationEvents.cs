using AG.Gameplay.Combat;
using System;

namespace AG.Gameplay.Systems
{
	public class ApplicationEvents
	{
		public event Action<AppState, AppState> OnAppStateChanged;
		public event Action OnLevelFinished;

		public void TriggerAppStateChanged(AppState oldAppState, AppState newAppState)
		{
			OnAppStateChanged?.Invoke(oldAppState, newAppState);
		}

		public void TriggerLevelFinished()
		{
			OnLevelFinished?.Invoke();
		}
	}
}
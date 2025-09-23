using AG.Gameplay.Combat;
using System;

namespace AG.Gameplay.Systems
{
	public class ApplicationEvents
	{
		public event Action<AppState, AppState> OnAppStateChanged;

		public event Action<GameplayFlow> OnBattleCreated;
		public event Action<GameplayFlow> OnBattleStarted;
		public event Action<GameplayFlow> OnBattleEnded;


		public void TriggerBattleCreated(GameplayFlow gameplayFlow)
		{
			OnBattleCreated?.Invoke(gameplayFlow);
		}

		public void TriggerBattleStarted(GameplayFlow gameplayFlow)
		{
			OnBattleStarted?.Invoke(gameplayFlow);
		}

		public void TriggerBattleEnded(GameplayFlow gameplayFlow)
		{
			OnBattleEnded?.Invoke(gameplayFlow);
		}

		public void TriggerAppStateChanged(AppState oldAppState, AppState newAppState)
		{
			OnAppStateChanged?.Invoke(oldAppState, newAppState);
		}

	}
}
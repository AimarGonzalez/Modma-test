using AG.Gameplay.Combat;
using System;

namespace AG.Gameplay.Systems
{
	public class ApplicationEvents
	{

		public event Action<GameplayFlow> OnBattleCreated;
		public event Action<GameplayFlow> OnBattleStarted;
		public event Action<GameplayFlow> OnBattleEnded;

		public event Action<ApplicationFlow.State, ApplicationFlow.State> OnGameStateChanged;

		public void TriggerBattleCreated(GameplayFlow gameplayFlow)
		{
			OnBattleCreated?.Invoke(gameplayFlow);
		}

		public void TriggerBattleStarted(GameplayFlow gameplayFlow)
		{
			OnBattleStarted?.Invoke(gameplayFlow);
		}

		public void TriggerBattlePaused(GameplayFlow gameplayFlow)
		{
			OnBattleEnded?.Invoke(gameplayFlow);
		}

		public void TriggerGameStateChanged(ApplicationFlow.State oldState, ApplicationFlow.State newState)
		{
			OnGameStateChanged?.Invoke(oldState, newState);
		}

	}
}
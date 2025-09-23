using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using AG.Gameplay.Characters.MonoBehaviours.Components;
using VContainer;


namespace AG.Gameplay.UI
{
	public class GameStateEnumBasedUI : EnumBasedUI<AppState>
	{
		[Inject]
		private ApplicationEvents _appEvents;

		private void Awake()
		{
			_appEvents.OnAppStateChanged += AppStateChanged;
		}

		private void OnDestroy()
		{
			_appEvents.OnAppStateChanged -= AppStateChanged;
		}

		private void AppStateChanged(AppState oldAppState, AppState newAppState)
		{
			base.OnStateChanged(oldAppState, newAppState);
		}
	}
}

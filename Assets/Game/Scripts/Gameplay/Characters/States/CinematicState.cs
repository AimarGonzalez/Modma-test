

using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using SharedLib.StateMachines;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class CinematicState : StateSubComponent
	{

		// ------------- Components -------------
		private PlayerAnimations _playerAnimations;
		private Character _character;

		// ------------- Dependencies -------------

		[Inject] private ApplicationEvents _appEvents;


		private void Awake()
		{
			_playerAnimations = Root.Get<PlayerAnimations>();
			_character = Root.Get<Character>();
		}

		public override void OnEnterState()
		{
			_playerAnimations.PlayAimingIdle();

			_appEvents.OnAppStateChanged += OnAppStateChanged;
		}

		public override void OnExitState()
		{
			_playerAnimations.PlayMove();

			_appEvents.OnAppStateChanged -= OnAppStateChanged;
		}
		public override IState.Status UpdateState()
		{
			return IState.Status.Running;
		}
		
		private void OnAppStateChanged(AppState oldAppState, AppState newAppState)
		{
			switch (newAppState)
			{
				case AppState.Battle:
					_character.Fight();
					break;
			}
		}
	}
}

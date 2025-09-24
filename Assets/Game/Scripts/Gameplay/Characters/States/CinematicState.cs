

using SharedLib.StateMachines;

namespace AG.Gameplay.Characters.Components
{
	public class CinematicState : StateSubComponent
	{

		// ------------- Components -------------
		private PlayerAnimations _playerAnimations;

		private void Awake()
		{
			_playerAnimations = Root.Get<PlayerAnimations>();
		}

		public override void OnEnterState()
		{
			_playerAnimations.PlayIdle();
		}

		public override void OnExitState()
		{
			_playerAnimations.PlayMove();
		}
		public override IState.Status UpdateState()
		{
			return IState.Status.Running;
		}
	}
}

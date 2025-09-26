

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

		// ------------- Dependencies -------------

		[Inject] private ApplicationEvents _appEvents;


		private void Awake()
		{
			_playerAnimations = Root.Get<PlayerAnimations>();
		}

		public override void OnEnterState()
		{
			_playerAnimations.PlayAimingIdle();
		}

		public override void OnExitState()
		{
		}
		public override IState.Status UpdateState()
		{
			return IState.Status.Running;
		}
	}
}

using SharedLib.StateMachines;

namespace AG.Gameplay.Characters.Components
{
	public class EnemyCinematicState : StateSubComponent
	{
		public override void OnEnterState()
		{
			//Just wait for all the other enemies to spawn animations to finish
			//Coordinated in LevelController.
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

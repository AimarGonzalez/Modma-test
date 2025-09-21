

namespace SharedLib.StateMachines
{
	public interface IState
	{
		public enum Status
		{
			Running,
			Finished
		}

		StateId StateId { get; }
		void InitState(StateMachine stateMachine);
		void OnEnterState();
		void OnExitState();
		Status UpdateState();
	}
}


using System;

namespace SharedLib.StateMachines
{
	public interface IState<TId, TStateMachine> where TId : struct, Enum
	{
		public enum Status
		{
			Running,
			Finished
		}

		TId StateId { get; }
		void InitState(TStateMachine stateMachine);
		void EnterState();
		void ExitState();
		IState.Status UpdateState();
	}
}
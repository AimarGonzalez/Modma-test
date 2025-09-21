

using System;

namespace SharedLib.StateMachines
{
	public interface IState<TStateId> where TStateId : struct, Enum
	{
		public enum Status
		{
			Running,
			Finished
		}

		TStateId StateId { get; }
		void InitState(StateMachine<TStateId> stateMachine);
		void EnterState();
		void ExitState();
		IState.Status UpdateState();
	}
}
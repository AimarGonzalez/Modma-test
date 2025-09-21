using SharedLib.ComponentCache;

using UnityEngine;

namespace SharedLib.StateMachines
{
	public abstract class StateSubComponent : SubComponent, IState
	{

		// ------------- Inspector fields -------------
		[SerializeField]
		private StateId _stateId;


		// ------------- Private fields ----------------
		private StateMachine _stateMachine;

		// ------------- Public properties -------------
		protected StateMachine StateMachine => _stateMachine;
		protected bool IsCurrentState => _stateMachine.CurrentState == this;

		StateId IState.StateId => _stateId;
		void IState.InitState(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public abstract void OnEnterState();
		public abstract void OnExitState();
		public abstract IState.Status UpdateState();
	}
}

